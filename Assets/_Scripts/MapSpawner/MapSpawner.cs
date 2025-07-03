using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSpawner : MonoBehaviour
{
    //Public
    public float scrollSpeed = 0f;
    public int distanceToNextMap = 200;
    public Transform StartPosition;
    public List<GameObject> mapPrefab;

    //Private
    private GameObject previousMap;
    private GameObject currentMap;
    private GameObject nextMap;
    private int currentMapIndex = 0;
    private int nextMapIndex = -1;
    private float currentMapLength;

    //Thêm prefab vào pool
    private void AddListPrefabToPoolAndSetParents()
    {
        ObjectPooler.Instance.Add("Map", mapPrefab);
        ObjectPooler.Instance.SetParents(gameObject, "Map");
    }
    //Khoi tạo Map
    private void InitMap()
    {
        currentMap = ObjectPooler.Instance.SpawnFromPool("Map", currentMapIndex, StartPosition.position, Quaternion.identity);
        currentMapLength = currentMap.GetComponent<MapInfo>().MapLength;

        nextMap = SpawnRandomMap(GetNextMapPosition());
    }
    void Start()
    {
        AddListPrefabToPoolAndSetParents();

        InitMap();
    }

    void Update()
    {
        MoveMap();
        UpdateMap();
    }

    //Cập nhật Map
    private void UpdateMap()
    {
        if (currentMap.transform.position.x <= StartPosition.position.x - currentMapLength / 2)
        {
            ObjectPooler.Instance.DesTroy(previousMap);

            previousMap = currentMap;

            currentMap = nextMap;
            currentMapLength = currentMap.GetComponent<MapInfo>().MapLength;

            nextMap = SpawnRandomMap(GetNextMapPosition());
        }
    }


    //Lấy vi trí của Map tiếp theo
    private Vector3 GetNextMapPosition()
    {
        return currentMap.transform.position + new Vector3(currentMapLength, 0, 0);
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
    void MoveMap()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
//Anh Khoa