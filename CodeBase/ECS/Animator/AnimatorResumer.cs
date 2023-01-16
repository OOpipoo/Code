using Leopotam.Ecs;

namespace Ecs.Animator
{
    public class AnimatorResumer : IEcsRunSystem
    {
        private EcsFilter<AnimatorContainer, ResumeAnimations> _filter;
        public void Run()
        {
            foreach (int i in _filter)
            {
                ref AnimatorContainer animatorContainer = ref _filter.Get1(i);
                animatorContainer.Animator.speed = 1;
                _filter.GetEntity(i).Del<ResumeAnimations>();
            }
        }
    }
}