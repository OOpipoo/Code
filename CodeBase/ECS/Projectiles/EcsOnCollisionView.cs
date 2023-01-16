using System;
using Enemy;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Projectiles
{
    public class EcsOnCollisionView : MonoBehaviour
    {
        public Transform GetTransform() => transform;
        private EcsEntity _entity;
        public void InjectEntity(EcsEntity entity)
        {
            _entity = entity;
        }

        public Action<EcsEntity> OnContactAction { get; set; }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EntityHolder entityHolder))
            {
                OnContactAction?.Invoke(entityHolder.EcsEntity);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EntityHolder entityHolder))
            {
                OnContactAction?.Invoke(entityHolder.EcsEntity);
            }
        }
    }
}