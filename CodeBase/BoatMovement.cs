using System;
using System.Reflection.Emit;
using AmazingAssets.CurvedWorld;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers
{
	public class BoatMovement : MonoBehaviour
	{
		[SerializeField] private Transform _centerPoint;
		[SerializeField] private int _speed = 2000;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _cameraPoint;
		[SerializeField] private Transform _wheelPoint;
		private IInputService _inputService;
		private Rigidbody _rb;
		private Vector3 _inputVector;
		

		public Vector3 GetSpawnPoint() => _spawnPoint.position;
		public Transform GetCameraPoint() => _cameraPoint.transform;
		public Transform GetWheelPoint() => _wheelPoint.transform;
		public int GetBoatSpeed() => _speed;

		public void UpdateBoatState(int increasedSpeed)
		{
			_speed += increasedSpeed;
		}

		public void SetBoatSpeed(int value) => _speed = value;
		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
		}

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}

		private void FixedUpdate()
		{
			Movement();
		}

		private void Movement()
		{
			Vector3 input = _inputService.GetMovementDirection();
			
			_inputVector = new Vector3(input.x, 0, input.z);
			if (_inputVector.magnitude > Mathf.Epsilon)
			{
				Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation,
					Quaternion.LookRotation(new Vector3(_inputVector.x, 0, _inputVector.z)),
					Time.fixedDeltaTime * 300);
				RotateAround(transform, _centerPoint, targetRotation);
			}
			_rb.velocity = transform.forward * _inputVector.magnitude * (Time.fixedDeltaTime * _speed) / 2;
			_rb.angularVelocity = Vector3.zero;
		}
		static void RotateAround (Transform transform, Transform pivotPoint, Quaternion rot)
		{
			transform.position = pivotPoint.position;
			transform.rotation = rot;
			transform.position += (transform.position - pivotPoint.position);
		}
		
		
	}
}
