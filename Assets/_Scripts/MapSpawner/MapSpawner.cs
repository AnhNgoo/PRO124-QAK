using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSpawner : MonoBehaviour
{
    //Public
    public float scrollSpeed = 0f;

    public List<GameObject> mapPrefab;

    //Private
    private Vector3 StartSpawnMapPoint = new Vector3(-1.7f, 0, 0);
    private GameObject previousMap;
    private GameObject currentMap;
    private GameObject nextMap;
    private int currentMapIndex = 0;
    private int nextMapIndex = -1;

    //Thêm prefab vào pool
    private void AddListPrefabToPoolAndSetParents()
    {
        ObjectPooler.Instance.Add("Map", mapPrefab);
        ObjectPooler.Instance.SetParents(gameObject, "Map");
    }
    //Khoi tạo Map
    private void InitMap()
    {

        currentMap = ObjectPooler.Instance.SpawnFromPool("Map", currentMapIndex, StartSpawnMapPoint, Quaternion.identity);
        nextMap = SpawnRandomMap(GetNextMapPosition());
        nextMap.GetComponent<ResetMap>().Reset();
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
        if (currentMap.transform.position.x <= StartSpawnMapPoint.x)
        {
            ObjectPooler.Instance.DesTroy(previousMap);

            previousMap = currentMap;

            currentMap = nextMap;

            nextMap = SpawnRandomMap(GetNextMapPosition());
            nextMap.GetComponent<ResetMap>().Reset();
        }
    }


    //Lấy vi trí của Map tiếp theo
    private Vector3 GetNextMapPosition()
    {
        Vector3 SpawnMapPoint = Vector3.zero;
        foreach (Transform child in currentMap.transform)
        {
            if (child.CompareTag("SpawnMapPoint"))
            {
                SpawnMapPoint = child.position;
                break;
            }
        }
        return SpawnMapPoint;
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
            currentMapIndex = nextMapIndex;
        nextMapIndex = newIndex;
    }
    //Di chuyển Map
    void MoveMap()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
//Anh Khoa