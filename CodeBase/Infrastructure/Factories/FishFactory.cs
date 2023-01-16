using System.Collections.Generic;
using System.Linq;
using CodeBase.Fishing;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
	public class FishFactory
	{
		private Dictionary<FishType, Fish> _fishPrefab;

		public FishFactory() =>
			_fishPrefab = Resources
				.LoadAll<Fish>(AssetPath.Fishes)
				.ToDictionary(x => x.FishType, x => x);

		public Fish Create(FishType fishType, Vector3 at) => 
			Object.Instantiate(_fishPrefab[fishType], at, Quaternion.identity);
	}
}