using System.Collections.Generic;
using System.Linq;
using EBunny.Scripts;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.Levels
{
    public class LevelService
    {
        private List<LevelPrefab> _levelList = new();
        private GameObject _currentLevel;
        private DiContainer _diContainer;

        public LevelService(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _levelList = Resources.LoadAll<LevelPrefab>(AssetPath.Levels)
                .OrderBy(x => x.name)
                .ToList();
        } 
        
        public LevelPrefab LoadLevelPrefab()
        {
            var level = _levelList[LevelInfoContainer.CurrentLevel];
            if (_currentLevel != null)
            {
                Object.DestroyImmediate(_currentLevel);
            }
            _currentLevel = _diContainer.InstantiatePrefab(level);
            return level;
        }
    }
}
