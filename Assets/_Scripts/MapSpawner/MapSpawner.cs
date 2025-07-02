using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSpawner : MonoBehaviour
{
    //Public
    public float scrollSpeed = 0f;
    public int distanceToNextMap = 100;
    public Transform StartPosition;
    public List<GameObject> mapPrefab;

    //Private
    private GameObject previousMap;
    private GameObject currentMap;
    private GameObject nextMap;
    private int currentMapIndex = 0;
    private int nextMapIndex = -1;

    //Thêm prefab vào pool
    private void AddListPrefabToPool()
    {
        ObjectPooler.Instance.Add("Map", mapPrefab);
    }
    //Khoi tạo Map
    private void InitMap()
    {
        currentMap = ObjectPooler.Instance.SpawnFromPool("Map", currentMapIndex, StartPosition.position + new Vector3(-5, 0, 0), Quaternion.identity);
        nextMap = SpawnRandomMap(GetNextMapPosition());
    }
    void Start()
    {
        AddListPrefabToPool();
        ObjectPooler.Instance.SetParents(gameObject, "Map");
        InitMap();
    }

    void Update()
    {
        UpdateMap();
    }

    //Cập nhật Map
    private void UpdateMap()
    {
        MoveMap(currentMap);

        if (currentMap.transform.position.x <= -30)
        {
            ObjectPooler.Instance.DesTroy(previousMap);

            previousMap = currentMap;
            currentMap = nextMap;
            nextMap = SpawnRandomMap(GetNextMapPosition());
        }
    }


    //Lấy vi trí của Map tiếp theo
    private Vector3 GetNextMapPosition()
    {
        return currentMap.transform.position + new Vector3(distanceToNextMap, 0, 0);
    }

    //Spawn Map ngẫu nhiên
    private GameObject SpawnRandomMap(Vector3 position)
    {
        int newIndex = RandomIndex();

        GameObject obj = ObjectPooler.Instance.SpawnFromPool("Map", newIndex, position, Quaternion.identity);

        AssignIndexForCurrentMapAndNextMap(newIndex);

        return obj;
    }

    private int RandomIndex()
    {
        int index;
        do
        {
            index = Random.Range(1, mapPrefab.Count);
        }
        while (index == currentMapIndex || index == nextMapIndex);

        return index;
    }

    //Gán index cho Map hiện tại và Map tiếp theo
    private void AssignIndexForCurrentMapAndNextMap(int newIndex)
    {
        // Gán lại index phù hợp
        if (currentMap == null)
            currentMapIndex = newIndex;
        else if (nextMap == null)
            nextMapIndex = newIndex;
        else
            currentMapIndex = nextMapIndex; // shift index theo logic map
        nextMapIndex = newIndex;
    }
    //Di chuyển Map
    void MoveMap(GameObject map)
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
//Anh Khoa