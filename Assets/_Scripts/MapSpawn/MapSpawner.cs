using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : Singleton<MapSpawner>
{
    //Public
    public float scrollSpeed = 0f;
    public float scrollSpeedLimit = 35f; // Giới hạn tốc độ cuộn
    public float durationStop = 1f;
    public float increaseAmount = 0.1f; // Tăng tốc độ cuộn
    public Vector3 StartSpawnMapPoint = new Vector3(-5f, 0, 0);
    public List<GameObject> mapPrefab;

    //Private
    private PlayerController playerController;

    public GameObject currentMap { get; set; }
    public GameObject nextMap { get; set; }
    private int currentMapIndex = 0;

    private float countdown = 0f; // Biến đếm thời gian để tăng tốc độ

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
        currentMap.name = "StartingMap";
        nextMap = SpawnRandomMap(GetNextMapPosition());
    }

    public void SetScrollSpeed(float speed) => scrollSpeed = speed;
    void Start()
    {
        AddListPrefabToPoolAndSetParents();
        InitMap();
        RegisterEvents();
    }

    void Update()
    {
        MoveMap();
        UpdateMap();
        IncreaseScrollSpeed();
    }

    private void RegisterEvents()
    {
        GameEvent.Instance.RegisterEvent("PlayerDeath", StopScrollingInTime);
    }
    //Cập nhật Map
    private void UpdateMap()
    {
        if (nextMap.transform.position.x <= StartSpawnMapPoint.x)
        {
            ObjectPooler.Instance.DesTroy(currentMap);

            currentMap = nextMap;

            nextMap = SpawnRandomMap(GetNextMapPosition());
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
        while (index == currentMapIndex);

        return index;
    }

    //Gán index cho Map hiện tại và Map tiếp theo
    private void AssignIndexForCurrentMapAndNextMap(int newIndex) => currentMapIndex = newIndex;
    //Di chuyển Map
    void MoveMap() => transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

    //Dừng khi player chết
    public void StopScrollingInTime() => StartCoroutine(SmoothStopScroll(durationStop));


    // Dừng cuộn mượt mà
    private IEnumerator SmoothStopScroll(float durationStop)
    {
        float startSpeed = scrollSpeed;
        float timeElapsed = 0f;

        while (timeElapsed < durationStop)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / durationStop;
            scrollSpeed = Mathf.Lerp(startSpeed, 0f, t);
            yield return null;
        }

        scrollSpeed = 0f;
    }

    // Tăng tốc độ cuộn
    private void IncreaseScrollSpeed()
    {
        if (scrollSpeed == 0 || scrollSpeed == scrollSpeedLimit) return;


        countdown += Time.deltaTime;
        if (countdown < 1) return;
        countdown = 0f; // Reset countdown

        scrollSpeed += increaseAmount;
    }


}
//Anh Khoa