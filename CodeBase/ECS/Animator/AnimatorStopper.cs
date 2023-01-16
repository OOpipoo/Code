using Ecs.Abilities.Stun;
using Leopotam.Ecs;

namespace Ecs.Animator
{
    public class AnimatorStopper : IEcsRunSystem
    {
        private EcsFilter<AnimatorContainer,AddStun> _filter;
        public void Run()
        {
            foreach (int i in _filter)
            {
                ref AnimatorContainer animator = ref _filter.Get1(i);
                animator.Animator.speed = 0;
            }
        }
    }
}