using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    private readonly Dictionary<string, List<GameObject>> poolDictionary = new();
    private readonly Dictionary<string, Queue<GameObject>> queueDictionary = new(); // Queue thật sự

    public void Add(string tag, List<GameObject> listPrefab)
    {
        List<GameObject> objectPool = new();
        Queue<GameObject> objectQueue = new();

        foreach (var prefab in listPrefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
            objectQueue.Enqueue(obj); // Add vào queue
        }

        poolDictionary.Add(tag, objectPool);
        queueDictionary.Add(tag, objectQueue);
    }

    // Spawn từ queue - lấy object đầu tiên ra và add vào cuối queue
    public GameObject SpawnFromQueue(string tag, Vector3 position, Quaternion rotation)
    {
        if (!queueDictionary.ContainsKey(tag) || queueDictionary[tag].Count == 0)
            return null;

        Queue<GameObject> queue = queueDictionary[tag];

        // Dequeue object đầu tiên
        GameObject obj = queue.Dequeue();

        // Setup object
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);

        // Enqueue lại vào cuối để tái sử dụng
        queue.Enqueue(obj);

        return obj;
    }

    // Spawn từ queue với auto disable
    public GameObject SpawnFromQueue(string tag, Vector3 position, Quaternion rotation, float autoDisableTime)
    {
        GameObject obj = SpawnFromQueue(tag, position, rotation);

        if (obj != null)
        {
            StartCoroutine(AutoDisable(obj, autoDisableTime));
        }

        return obj;
    }

    // Return object về queue ngay lập tức
    public void ReturnToQueue(string tag, GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);

        // Không cần làm gì thêm vì object đã ở trong queue rồi
    }

    // Kiểm tra queue có trống không
    public bool IsQueueEmpty(string tag)
    {
        return !queueDictionary.ContainsKey(tag) || queueDictionary[tag].Count == 0;
    }

    // Lấy số lượng objects trong queue
    public int GetQueueCount(string tag)
    {
        return queueDictionary.ContainsKey(tag) ? queueDictionary[tag].Count : 0;
    }

    // Existing methods giữ nguyên...
    public GameObject SpawnFromPool(string tag, int index, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = poolDictionary[tag][index];
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, float autoDisableTime)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.SetPositionAndRotation(position, rotation);
                StartCoroutine(AutoDisable(obj, autoDisableTime));
                return obj;
            }
        }

        return null;
    }

    public void DesTroy(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
    }

    public void SetParents(GameObject parent, string tag)
    {
        foreach (var obj in poolDictionary[tag])
        {
            obj.transform.SetParent(parent.transform);
        }
    }

    private IEnumerator AutoDisable(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
