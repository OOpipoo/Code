using Ecs.Destroy;
using Ecs.Helper;
using Ecs.TimerBehaviour;
using Leopotam.Ecs;

namespace Ecs.Projectiles.Pool
{
    public class ReturnProjectileToPool : IEcsRunSystem
    {
        private EcsFilter<ReturnECSCollisionToPool, Timer> _filter;
        public void Run()
        {
            foreach (int i in _filter)
            {
                ref Timer timer = ref _filter.Get2(i);
                if (timer.IsTimeUp())
                {
                    ref ReturnECSCollisionToPool returnToPool = ref _filter.Get1(i);
                    ref EcsEntity entity = ref returnToPool.Target;
                    ref var preDestroyFlag = ref entity.Get<PreDestroyFlag>();
                    ref Projectile projectile = ref entity.Get<Projectile>();
                    if (projectile.ProjectileEntity != null)
                    {
                        returnToPool.Pool.Return(projectile.ProjectileEntity);
                    }
                    ref EcsEntity currentEntity = ref _filter.GetEntity(i);
                    entity.RemoveFromDestroyable(currentEntity);
                    currentEntity.Destroy();
                }
            }
        }
    }
}