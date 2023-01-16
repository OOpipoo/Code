using Ecs.Flags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Movement.StandartMovement
{
    public class MoveToPlayerSystem : IEcsRunSystem
    {
        private EcsFilter<SpeedComponent, TransformComponent, Active, MoveToPlayer, Movable> _movable;
        private EcsFilter<TransformComponent, PlayerFlag> _player;
        public void Run()
        {
            if (_player.IsEmpty()) return;
            Vector3 targetPosition = _player.Get1(0).Transform.position;
            foreach (var i in _movable)
            {
                ref SpeedComponent speed = ref _movable.Get1(i);
                ref TransformComponent transformComponent = ref _movable.Get2(i);
                Vector3 direction = targetPosition - transformComponent.Transform.position;
                ref MoveComponent moveComponent = ref _movable.GetEntity(i).Get<MoveComponent>();
                moveComponent.Direction = direction.normalized;
                moveComponent.Speed = speed.Speed;
            }
        }
    }
}