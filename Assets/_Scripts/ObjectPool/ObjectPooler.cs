using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private readonly Dictionary<string, List<GameObject>> poolDictionary = new();
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Add(string tag, List<GameObject> listPrefab)
    {
        List<GameObject> objectPool = new();

        foreach (var prefab in listPrefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        poolDictionary.Add(tag, objectPool);
    }

    public GameObject SpawnFromPool(string tag, int index, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = poolDictionary[tag][index];
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }
    public void DesTroy(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
    }

    public void SetParents(GameObject parent ,string tag)
    {
        foreach (var obj in poolDictionary[tag])
        {
            obj.transform.SetParent(parent.transform);
        }
    }
}
