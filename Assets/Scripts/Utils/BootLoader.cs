using UnityEngine;
using UnityEngine.AddressableAssets;

public class BootLoader : MonoBehaviour
{
    public AssetReference persistent;

    private void Awake()
    {
        Addressables.LoadSceneAsync(persistent);
    }
}
