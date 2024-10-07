using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck:false,
            defaultCapacity:10,
            maxSize:20
        );

        PreFillPool(7);
    }

    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        { 
            pool.Release(item); 
        }
    }

    public GameObject GetObjectFromPool() 
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }

    public void Reset(GameObject obj)
    {
        pool.Release(obj);
    }
}
