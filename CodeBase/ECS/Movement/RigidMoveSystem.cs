using Ecs.Abilities.Stun;
using Ecs.Attack;
using Ecs.Flags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Movement
{
    public class RigidMoveSystem : IEcsRunSystem
    {
        private EcsFilter<MoveComponent, RigidbodyComponent, Active, Movable> _movable;
        private EcsFilter<RigidbodyComponent,MoveComponent, Active>.Exclude<Movable> _nonMovable;
        public void Run()
        {
            foreach (var i in _movable)
            {
                ref MoveComponent move = ref _movable.Get1(i);
                ref RigidbodyComponent rigidbody = ref _movable.Get2(i);
                rigidbody.Rigidbody.AddForce(move.Direction, ForceMode.VelocityChange);
                rigidbody.Rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.Rigidbody.velocity, move.Speed);
                rigidbody.Rigidbody.mass = 1;
            }

            foreach (int i in _nonMovable)
            {
                ref RigidbodyComponent rigidbody = ref _nonMovable.Get1(i);
                rigidbody.Rigidbody.velocity = Vector3.zero;
                rigidbody.Rigidbody.mass = 2;
            }
        }
    }
}