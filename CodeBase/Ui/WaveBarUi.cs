using System.Linq;
using CodeBase.Infrastructure.Services.EnemySpawner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Ui
{
	public class WaveBarUi : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private TextMeshProUGUI _currentValueText;
		private EnemySpawnerService _enemySpawnerService;


		[Inject]
		private void Construct(EnemySpawnerService enemySpawnerService) => 
			_enemySpawnerService = enemySpawnerService;

		private void LateUpdate()
		{
			int currentIndex = _enemySpawnerService.CurrentLevel.Waves.IndexOf(_enemySpawnerService.CurrentEnemyWave) + 1;
			int wavesCount = _enemySpawnerService.CurrentLevel.Waves.Count;
			_image.fillAmount = (float) currentIndex / wavesCount;
			_currentValueText.text = $"{currentIndex}/{wavesCount}";
		}
	}
}
