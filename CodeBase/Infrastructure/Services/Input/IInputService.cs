using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector3 GetMovementDirection();
        void DisableJoy();
        void EnableJoy();
    }
}
