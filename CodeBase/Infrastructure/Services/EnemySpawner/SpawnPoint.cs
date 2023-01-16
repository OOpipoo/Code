using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.Services.EnemySpawner
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _range = 3;
        private EnemySpawnerService _enemySpawnService;
        [SerializeField] private Button _button;

        [Inject]
        private void Construct(EnemySpawnerService enemySpawnerService)
        {
            _enemySpawnService = enemySpawnerService;
        }

        [Button]
        private void StartSpawnEnemies()
        {
            _enemySpawnService.StartSpawn();
        }
        
        [Button]
        private void StopSpawnEnemies()
        {
            _enemySpawnService.StopSpawn();
        }

        public Vector3 GetRandomPosition()
        {
            var randomX = UnityEngine.Random.Range(-_range, _range);
            var randomZ = UnityEngine.Random.Range(-_range, _range);
            return new Vector3(randomX, 0, randomZ) + transform.position;
        }
        private void OnDrawGizmos() => 
            Gizmos.DrawWireSphere(transform.position, _range);
    }
}