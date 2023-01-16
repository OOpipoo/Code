using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.AssetsManagement
{
    public class AssetProvider : IAssetProvider
    {
        public List<T> LoadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path).ToList();
        }

        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}
