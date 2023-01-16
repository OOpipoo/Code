using Ecs.Helper;
using Leopotam.Ecs;

namespace Ecs.Projectiles.Pool
{
    public struct ReturnECSCollisionToPool
    {
        public EcsEntity Target;
        public SimplePool<EcsOnCollisionView> Pool;
    }
}