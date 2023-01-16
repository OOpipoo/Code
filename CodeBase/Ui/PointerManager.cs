using System;
using System.Collections.Generic;
using CodeBase.Player;
using CodeBase.Utils;
using EBunny.Scripts;
using UnityEngine;

namespace CodeBase.Ui
{
	// [ExecuteAlways]
	public class PointerManager : MonoBehaviour
	{
		[SerializeField] private Transform _playerTransform;
		[SerializeField] private Camera _camera;
		[SerializeField] private PointerIcon _pointerPrefab;

		private readonly Dictionary<EnemyPointer, PointerIcon> _dictionary = new();

		public static PointerManager Instance;


		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else
				Destroy(this);
		}

		private void Start()
		{
			_camera = Camera.main;
			
			Communicator.Instance.OnGameStart += () => 
			_playerTransform = FindObjectOfType<PlayerMovement>(true).transform;
		}

		public void AddToList(EnemyPointer enemyPointer)
		{
			var newPointer = Instantiate(_pointerPrefab, transform);
			_dictionary.Add(enemyPointer, newPointer);
		}

		public void RemoveFromList(EnemyPointer enemyPointer)
		{
			Destroy(_dictionary[enemyPointer].gameObject);
			_dictionary.Remove(enemyPointer);
		}

		private void LateUpdate()
		{
			var planes = GeometryUtility.CalculateFrustumPlanes(_camera);

			foreach (var kvp in _dictionary)
			{
				var enemyPointer = kvp.Key;
				var pointerIcon = kvp.Value;

				var toEnemy = enemyPointer.transform.position - _playerTransform.position;
				var ray = new Ray(_playerTransform.position, toEnemy);
				Debug.DrawRay(_playerTransform.position, toEnemy);
				
				var rayMinDistance = Mathf.Infinity;
				var index = 0;

				for (var p = 0; p < 4; p++)
					if (planes[p].Raycast(ray, out var distance))
						if (distance < rayMinDistance)
						{
							rayMinDistance = distance;
							index = p;
						}

				rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toEnemy.magnitude);
				var worldPosition = ray.GetPoint(rayMinDistance);
				var position = _camera.WorldToScreenPoint(worldPosition);
				var rotation = GetIconRotation(index);
				
				if (toEnemy.magnitude > rayMinDistance)
					pointerIcon.Show();
				else
					pointerIcon.Hide();

				pointerIcon.SetIconPosition(position, rotation);
				
			}
		}

		private Quaternion GetIconRotation(int planeIndex)
		{
			if (planeIndex == 0)
				return Quaternion.Euler(0f, 0f, 90f);
			if (planeIndex == 1)
				return Quaternion.Euler(0f, 0f, -90f);
			if (planeIndex == 2)
				return Quaternion.Euler(0f, 0f, 180);
			if (planeIndex == 3) return Quaternion.Euler(0f, 0f, 0f);
			return Quaternion.identity;
		}

	}

}