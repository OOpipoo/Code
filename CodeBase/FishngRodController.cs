using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Fishing;
using CodeBase.Infrastructure.Services.Fishing;
using CodeBase.Utils;
using CodeBase.ZonesAndTriggers;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers
{
	public class FishingRodController : MonoBehaviour
	{
		[SerializeField] private GameObject _collectZone;
		[SerializeField] private GameObject _fishingZone;
		[SerializeField] private Transform _fishingNet;
		[SerializeField] private Transform _fishingRope;
		[SerializeField] private float _durationTime = 1.5f;
		[SerializeField] private Follower _follower;
		private List<OnWaterFishingTriggerZone> _fishingTriggerZoneList;
		public FloatReactiveProperty Progress = new(0);
		private NetContainer _netContainer;
		private FishingService _fishingService;
		private Sequence _fishingSequence;
		private float _netStartPosition;
		private float _ropeStartPosition;


		private void Start()
		{
			_netContainer = gameObject.GetComponent<NetContainer>();
			_netStartPosition = _fishingNet.localPosition.y;
			_ropeStartPosition = _fishingRope.localPosition.y;
			_collectZone.SetActive(false);
			_fishingTriggerZoneList = FindObjectsOfType<OnWaterFishingTriggerZone>().ToList();
			foreach (var zone in _fishingTriggerZoneList)
			{
				zone.OnWaterFishingTrigger.AddListener(() => OnOffFishingZoneOnBoat(true));
			}
			OnOffFishingZoneOnBoat(false);
		}

		[Inject]
		private void Construct(FishingService fishingService)
		{
			_fishingService = fishingService;
		}

		public void OnOffFishingZoneOnBoat(bool switcher)
		{
			_fishingZone.SetActive(switcher);
		}

		public void StartFishingAndLowerCage()
		{
			_collectZone.SetActive(false);
			_fishingRope.DOScaleY(2.5f, _durationTime).SetEase(Ease.Linear);
			_fishingSequence = DOTween.Sequence()
				.Append(_fishingNet.DOLocalMoveY(-0.1f, _durationTime)
					.SetLoops(1, LoopType.Yoyo)
					.SetEase(Ease.Linear))
				.AppendCallback(() => _fishingService.StartFishing(_netContainer, _follower, Progress));
			StartCoroutine(RaiseCage());
		}

		public void ExpandRodStartFishing()
		{
			gameObject.transform.DOLocalRotate(new Vector3(gameObject.transform.rotation.x, 0f,
					gameObject.transform.rotation.z), 1)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					_collectZone.SetActive(false);
					StartFishingAndLowerCage();
				});
		}

		public void RiseCage()
		{
			_fishingRope.DOScaleY(_ropeStartPosition, _durationTime)
				.SetEase(Ease.Linear);
			_fishingNet.DOLocalMoveY(_netStartPosition, _durationTime)
				.SetLoops(1, LoopType.Yoyo)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					gameObject.transform.DOLocalRotate(new Vector3(gameObject.transform.rotation.x, 0f,
						gameObject.transform.rotation.z), 1);
					_collectZone.SetActive(false);
				});

			_fishingSequence.Kill();
			Progress.Value = 0;
			_netContainer.ClearCageDictionary();
			DestroyAllFishInNet();
		}

		private IEnumerator RaiseCage()
		{
			yield return new WaitUntil(() => _netContainer.IsFull);
			_fishingRope.DOScaleY(_ropeStartPosition, _durationTime).SetEase(Ease.Linear);
			_fishingNet.DOLocalMoveY(_netStartPosition, _durationTime)
				.SetLoops(1, LoopType.Yoyo)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					gameObject.transform.DOLocalRotate(new Vector3(gameObject.transform.rotation.x, 90f,
							gameObject.transform.rotation.z), 1)
						.SetEase(Ease.Linear);

					_collectZone.SetActive(true);
				});
		}

		private void DestroyAllFishInNet()
		{
			foreach (Transform child in _follower.transform)
			{
				Destroy(child.gameObject);
			}
		}

		private void OnDisable()
		{
			foreach (var zone in _fishingTriggerZoneList)
			{
				zone.OnWaterFishingTrigger.RemoveListener(() => OnOffFishingZoneOnBoat(true));
			}
		}
	}
}
