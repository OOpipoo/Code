using Ecs.Animator;
using Ecs.Flags;
using Ecs.Movement.Flags;
using Ecs.TimerBehaviour;
using Leopotam.Ecs;

namespace Ecs.Abilities.Stun
{
    public class StunRemover : IEcsRunSystem
    {
        private EcsFilter<StunComponent> _stunFilter;
        public void Run()
        {
            foreach (int i in _stunFilter)
            {
                ref StunComponent stunComponent = ref _stunFilter.Get1(i);
                ref Timer stunTimer = ref stunComponent.TimerEntity.Get<Timer>();
                if (stunTimer.EstimateTime <= 0)
                {
                    stunComponent.TimerEntity.Destroy();
                    ref EcsEntity entity = ref _stunFilter.GetEntity(i);
                    entity.Get<ResumeAnimations>();
                    entity.Del<StunComponent>();
                    entity.Get<RevertMovable>();
                }
            }
        }
    }
}