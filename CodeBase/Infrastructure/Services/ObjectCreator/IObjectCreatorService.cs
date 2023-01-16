using UnityEngine;

namespace CodeBase.Infrastructure.Services.ObjectCreator
{
    public interface IObjectCreatorService
    {
        GameObject Instantiate(GameObject gameObject, Vector3 at, Transform parent = null);
        
        GameObject InstantiateWithRegister(string assetPath);
        
        GameObject InstantiateWithRegister(GameObject gameObject, Vector3 at, Transform parent = null);
        
        GameObject InstantiateWithRegister(string assetPath, Vector3 at, Transform parent = null);
        
        T InstantiateWithRegisterComponent<T>(string from, Vector3 at);
        
        T InstantiateWithRegisterComponent<T>(string from);
    }
}
