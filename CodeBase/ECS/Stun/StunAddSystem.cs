using Ecs.Flags;
using Ecs.TimerBehaviour;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Abilities.Stun
{
    public class StunAddSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<AddStun> _filter;
        public void Run()
        {
            foreach (int i in _filter)
            {
                ref AddStun addStun = ref _filter.Get1(i);
                ref EcsEntity entity = ref _filter.GetEntity(i);
                if (entity.Has<StunComponent>())
                {
                    ref StunComponent stunComponent = ref entity.Get<StunComponent>();
                    ref Timer timer = ref stunComponent.TimerEntity.Get<Timer>();
                    timer.EstimateTime = Mathf.Max(timer.EstimateTime, addStun.Duration);
                }
                else
                {
                    EcsEntity newEntity = _world.NewEntity();
                    ref Timer timer = ref newEntity.Get<Timer>();
                    timer.EstimateTime = addStun.Duration;
                    entity.Get<StunComponent>() = new StunComponent
                    {
                        TimerEntity = newEntity
                    };
                    entity.Del<Movable>();
                }
                entity.Del<AddStun>();
            }
        }
    }
}