using Ecs.Flags;
using Ecs.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Animator
{
    public class AnimatorSpeedSystem : IEcsRunSystem
    {
        private static readonly int Speed = UnityEngine.Animator.StringToHash("Speed");
        private EcsFilter<RigidbodyComponent, AnimatorContainer, Active> _filter;
        private EcsFilter<RigidbodyComponent, AnimatorContainer>.Exclude<Active> _nonMovable;
        private int _counter;
        public void Run()
        {
            if (_counter++ < 4) return;
            _counter = 0;
            float deltaTime = Time.deltaTime;
            foreach (int i in _filter)
            {
                ref RigidbodyComponent rigidbodyComponent = ref _filter.Get1(i);
                float velocity = rigidbodyComponent.Rigidbody.velocity.magnitude;
                ref AnimatorContainer animatorContainer = ref _filter.Get2(i);
                rigidbodyComponent.LastVelocity = velocity;
                animatorContainer.Animator.SetFloat(Speed, velocity, 0.1f, deltaTime);
            }
            foreach (int i in _nonMovable)
            {
                ref AnimatorContainer animatorContainer = ref _nonMovable.Get2(i);
                animatorContainer.Animator.SetFloat(Speed, 0, 0.1f, deltaTime);
            }
        }
    }
}