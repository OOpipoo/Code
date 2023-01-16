using CodeBase.Controllers;
using CodeBase.Fishing;
using CodeBase.Infrastructure.Services.ObjectCreator;
using CodeBase.Utils;
using KinematicCharacterController;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.PlayerInstance
{
	public class PlayerInstanceService : IInitializable
	{
		private readonly Vector3 _playerSpawnPosition = new(-3f, 0.5f, -4f);
		private readonly IObjectCreatorService _creatorService;
		private VirtualCameraController _virtualCameraController;
		private GameObject _player;

		public PlayerInstanceService(IObjectCreatorService creatorService)
		{
			_virtualCameraController = Object.FindObjectOfType<VirtualCameraController>();
			_creatorService = creatorService;
		}
		
		public void Initialize()
		{
			InstantiatePlayer();
		}

		public Transform GetPlayerTransform()
		{
			return _player.transform;
		}
		
		private void InstantiatePlayer()
		{  
			_player = _creatorService.InstantiateWithRegister(AssetPath.Player);
			var motor = _player.GetComponentInChildren<KinematicCharacterMotor>();
			motor.SetPosition(_playerSpawnPosition);
			_virtualCameraController.SetTargetForCamera(_player.transform, 3);
			_player.GetComponent<PlayerSimpleStates>().UpdateState(PlayerState.OnBoat);
		}

		public FishCarrier GetFishCarrier()
		{
			var collector = _player.GetComponentInChildren<FishCarrier>();
			return collector;
		}
		
		public CapacityUi GetCapacityUi()
		{
			var capacity = _player.GetComponentInChildren<CapacityUi>();
			return capacity;
		}
		
		public int GetActiveChildIndex()
		{
			foreach (Transform child in _player.transform)
			{
				if (child.gameObject.activeSelf)
				{
					return child.GetSiblingIndex();
				}
			} 
			return 0;
		}

		public PlayerMovement GetPlayerMovement()
		{
			return _player.GetComponent<PlayerMovement>();
		}

		public void ChangeMeshForInstancePlayer(int onIndex)
		{
			_player.transform.GetChild(0).gameObject.SetActive(false);
			var active = _player.transform.GetChild(onIndex);
			active.gameObject.SetActive(true);
		}

		public PlayerSimpleStates GetState()
		{
			return _player.GetComponent<PlayerSimpleStates>();
		}
	}
}
