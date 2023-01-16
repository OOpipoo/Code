using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        public UltimateJoystick Joystick;
        private Vector3 _direction;

        private void Awake() => 
            Joystick = GetComponentInChildren<UltimateJoystick>();

        public void EnableJoy() => 
            Joystick.EnableJoystick();

        public void DisableJoy()
        {
            _direction = new Vector3(0,0,0);
            Joystick.DisableJoystick();
        }

        private void Update()
        {
            SetDirection();
        }

        public Vector3 GetMovementDirection() => _direction;

        private void SetDirection()
        {
            float z = Joystick.GetVerticalAxis();
            float x = Joystick.GetHorizontalAxis();
            _direction = new Vector3(x, 0, z);
        }
    }
}
