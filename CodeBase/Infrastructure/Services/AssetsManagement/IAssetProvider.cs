using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.AssetsManagement
{
    public interface IAssetProvider
    {
        List<T> LoadAll<T>(string path) where T : Object;
        T Load<T>(string path) where T : Object;
    }
}
