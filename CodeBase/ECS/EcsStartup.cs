using Ecs.Abilities.Stun;
using Ecs.Animator;
using Ecs.Attack;
using Ecs.Components.DamageDealer;
using Ecs.Components.Skills.Common;
using Ecs.Destroy;
using Ecs.Movement;
using Ecs.Movement.MovementThroughPlayer;
using Ecs.Movement.MovementWithStopping;
using Ecs.Movement.StandartMovement;
using Ecs.Projectiles;
using Ecs.Projectiles.Destroy;
using Ecs.Projectiles.Pool;
using Ecs.Rotation;
using Ecs.Systems;
using Ecs.Systems.Skills;
using Ecs.TimerBehaviour;
using Ecs.VFX;
using Infrastructure.Factories;
using Infrastructure.ObjectPools;
using Infrastructure.Services.Player;
using Infrastructure.Services.VFXText;
using Leopotam.Ecs;
using Zenject;

namespace Ecs
{
    public class EcsStartup : ITickable, IFixedTickable
    {
        public static EcsWorld World;
        private EcsSystems _systems;
        private EcsSystems _fixedUpdateSystems;

        [Inject]
        private void Construct(FXPool fxPool, EnemyFactory enemyFactory, PlayerStatsService playerStatsService, VFXTextService vfxTextService)
        {
            World = new EcsWorld();
            _systems = new EcsSystems(World);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(World);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _fixedUpdateSystems = new EcsSystems(World);
            _fixedUpdateSystems
                .Add(new RigidMoveSystem());
            OnStartSystems();
            _systems
                .Add(new EnemyOutOfViewWatcher())
                .Add(new BoomSkillSystem())
                .Add(new AttackEndDamageCreator())
                .Add(new VampireSkillSystem())
                .Add(new AttackRangeWatcher())
                .Add(new ExplosionSystem())
                .Add(new PushSystem())
                .Add(new DotSystem())
                .Add(new TimeReducer())
                .Add(new DamageAuraSystem())
                .Add(new DamageProcessSystem())
                .Add(new VFXDamageTextSystem())
                .Add(new DamageDealerSystem())
                .Add(new LookAtSystem())
                .Add(new RotationSystem())
                .Add(new DistanceAttackEndWatcher())
                .Add(new MovableRevertSystem())
                .Add(new MoveThroughPlayerSystem())
                .Add(new RotationTowardTargetSystem())
                .Add(new ChangeMoveThroughPlayerDirection())
                .Add(new RemoveMovementWhenAttack());
            AddVFX();
            AddAnimator();
            AddMovement();
            AddStun();
            Projectiles();
            OnEndSystems();
            
            _systems
                .OneFrame<AddVFX>()
                .OneFrame<DamageComponent>()
                .OneFrame<ExplosionComponent>()
                .OneFrame<PushComponent>()
                .Inject(fxPool)
                .Inject(enemyFactory)
                .Inject(vfxTextService)
                .Inject(playerStatsService);
            _systems.Init();
            _fixedUpdateSystems.Init();
        }
        public void Tick() => _systems.Run();
        public void FixedTick() => _fixedUpdateSystems.Run();


        private void Projectiles() =>
            _systems
                .Add(new EaseMovingSystem())
                .Add(new ReturnProjectileToPool())
                .Add(new DelayDestroyTransformSystem());

        private void OnStartSystems(){}

        private void OnEndSystems() =>
            _systems
                .Add(new DestroyFlagger())
                .Add(new DestroyableHoldersKiller())
                .Add(new DestroySystem());

        private void AddAnimator() =>
            _systems
                // .Add(new AnimatorStopper())
                // .Add(new AnimatorResumer())
                .Add(new AnimatorSpeedSystem());

        private void AddVFX() =>
            _systems
                .Add(new VFXRemover())
                .Add(new VFXAddSystem());

        private void AddStun()=>
            _systems
                .Add(new StunRemover())
                .Add(new StunAddSystem());

        private void AddMovement()=>
            _systems
                .Add(new MoveToPlayerSystem())
                .Add(new MoveAndStopSystem())
                .Add(new EnemyRotationSystem());
    }
}