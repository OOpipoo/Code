using AmazingAssets.CurvedWorld;
using CodeBase.Infrastructure.Services.Input;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers
{
    public class PlayerMovement : MonoBehaviour
    {
        private ExampleCharacterController _character;
        private PlayerCharacterInputs _inputs;
        private IInputService _inputService;
        private AnimatorController _animatorController;
        private Camera _camera;
        private CurvedWorldController _curvedWorldController;
        private KinematicCharacterMotor _motor;
        private ExampleCharacterController _controller;
        private Rigidbody _rb;

        public float GetMovementSpeed() =>  _controller.MaxStableMoveSpeed;
        public void SetMovementSpeed(float value) => _controller.MaxStableMoveSpeed = value;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        private void OnEnable()
        {
            _curvedWorldController = FindObjectOfType<CurvedWorldController>();
            _curvedWorldController.bendPivotPoint = transform;
            
            _motor = GetComponent<KinematicCharacterMotor>();
            _controller = GetComponent<ExampleCharacterController>();
            _rb = gameObject.GetComponent<Rigidbody>();
        }
        
        private void Awake()
        {
            _camera = Camera.main;
            _character = GetComponent<ExampleCharacterController>();
            _character.SetInputs(ref _inputs);
            _animatorController = GetComponent<AnimatorController>();
        }
        private void Update()
        {
            Movement();
            _animatorController.SetSpeed(_motor.Velocity.magnitude);
        }
        
        public void OnOffPlayerControllers(bool switcher)
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            _controller = GetComponent<ExampleCharacterController>();
            _rb = gameObject.GetComponent<Rigidbody>();

            _motor.enabled = switcher;
            _controller.enabled = switcher;
            if (switcher && _rb == null)
            {
               _rb = gameObject.AddComponent<Rigidbody>();
               _rb.useGravity = false;
               _rb.isKinematic = true;
            }
            else if(!switcher && _rb != null)
            {
                Destroy(_rb);
            }
        }
        
        private void Movement()
        {
            SetInternalInputs();
            _character.SetInputs(ref _inputs);
        }
        
        private void SetInternalInputs()
        {
            var rotation = _camera.transform.rotation.eulerAngles;
            rotation.x = 0;

            Vector3 input = Quaternion.Euler(rotation) * _inputService.GetMovementDirection();
            _inputs.MoveAxisForward = ProcessDirectionSpeed(input.z);
            _inputs.MoveAxisRight = ProcessDirectionSpeed(input.x);
        }

        private float ProcessDirectionSpeed(float value)
        {
            if (Mathf.Abs(value) < 0.1f)
            {
                return 0;
            }
            return Mathf.Lerp(0.5f, 1, Mathf.Abs(value)) * Mathf.Sign(value);
        }
        public void IncreasedPlayerMoveSpeed()
        {
            _controller.MaxStableMoveSpeed += 0.2f;
        }
    }
}
