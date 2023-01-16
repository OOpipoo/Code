using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.StaticData.EnemyWaves;
using EBunny.Scripts;
using UniRx;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.EnemySpawner
{
    public class EnemySpawnerService
    {
        public EnemyWave CurrentEnemyWave;
        public Level CurrentLevel;
        public int EstimateToSpawn;
        public readonly List<GameObject> Enemies = new();

        private readonly EnemyFactory _enemyFactory;
        private List<Level> _levels;
        private CompositeDisposable _disposables = new();

        public EnemySpawnerService(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _levels = Resources.LoadAll<Level>(AssetPath.LevelsWaves).OrderBy(x => x.name).ToList();
        }
       
        public void StartSpawn()
        {
            CurrentLevel = _levels[LevelInfoContainer.CurrentLevel];
            CurrentEnemyWave = CurrentLevel.Waves.First();
            EstimateToSpawn = CurrentEnemyWave.EnemiesInWave.Sum(x => x.Count);
            Observable
                .FromCoroutine(SpawnEnemies)
                .Subscribe()
                .AddTo(_disposables);
        }

        public void StopSpawn()
        {
            _disposables.Clear();
        }
        
        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(3f);
            List<SpawnPoint> levelSpawnPoints = Object.FindObjectsOfType<SpawnPoint>().ToList();
            foreach (EnemyWave enemyWave in CurrentLevel.Waves)
            {
                CurrentEnemyWave = enemyWave;
                EstimateToSpawn = enemyWave.EnemiesInWave.Sum(x => x.Count);
                foreach (EnemyInWaveContainer enemyInWaveContainer in enemyWave.EnemiesInWave)
                {
                    SpawnPoint targetPoint = levelSpawnPoints[Random.Range(0, levelSpawnPoints.Count)];
                    for (int i = 0; i < enemyInWaveContainer.Count; i++)
                    {
                        EstimateToSpawn--;
                        GameObject enemy = _enemyFactory.Create(enemyInWaveContainer.Enemy, targetPoint.GetRandomPosition());
                        Enemies.Add(enemy);
                        yield return new WaitForSeconds(.2f);
                    }
                }

                yield return new WaitWhile(() => Enemies.All(x => x == null) == false);
                Enemies.Clear();
            }

            yield return new WaitForSeconds(.5f);
            Communicator.Instance.Win();
        }

        public void DestroyLastEnemies()
        {
            foreach (var enemy in Enemies)
            {
                Object.Destroy(enemy);
            }
        }
    }
}