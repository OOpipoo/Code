using CodeBase.Infrastructure.Services.AssetsManagement;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.ObjectCreator
{
    public class ObjectCreatorService : IObjectCreatorService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _diContainer;

        public ObjectCreatorService(IAssetProvider assetProvider, DiContainer diContainer)
        {
            _assetProvider = assetProvider;
            _diContainer = diContainer;
        }

        public GameObject Instantiate(GameObject gameObject, Vector3 at, Transform parent = null) => 
            Object.Instantiate(gameObject, at, Quaternion.identity, parent);

        public GameObject InstantiateWithRegister(string assetPath) => 
            _diContainer.InstantiatePrefab(LoadAsset<GameObject>(assetPath));
        
        public GameObject InstantiateWithRegister(GameObject gameObject, Vector3 at, Transform parent = null) => 
            _diContainer.InstantiatePrefab(gameObject, at, Quaternion.identity, parent);
        
        public GameObject InstantiateWithRegister(string assetPath, Vector3 at,Transform parent = null)
        {
            return InstantiateWithRegister(LoadAsset<GameObject>(assetPath), at, parent);
        }

        public T InstantiateWithRegisterComponent<T>(string from, Vector3 at) =>
            InstantiateWithRegister(LoadAsset<GameObject>(from), at)
                .GetComponentInChildren<T>();
        
        public T InstantiateWithRegisterComponent<T>(string from) =>
            InstantiateWithRegister(LoadAsset<GameObject>(from), Vector3.zero)
                .GetComponentInChildren<T>();

        private T LoadAsset<T>(string assetPath) where T : Object => 
            _assetProvider.Load<T>(assetPath);
    }
}
