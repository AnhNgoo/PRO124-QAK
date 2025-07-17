using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSpawner : Singleton<MapSpawner>
{
    //Public
    public float scrollSpeed = 0f;
    public float scrollSpeedLimit = 35f; // Giới hạn tốc độ cuộn
    public float durationStop = 1f;
    public float increaseAmount = 0.1f; // Tăng tốc độ cuộn


    public List<GameObject> mapPrefab;

    //Private
    private PlayerController playerController;
    private Vector3 StartSpawnMapPoint = new Vector3(-1.7f, 0, 0);
    private GameObject previousMap;
    public GameObject currentMap { get; set; }
    private GameObject nextMap;
    private int currentMapIndex = 0;
    private int nextMapIndex = -1;

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

    public void SetScrollSpeed(float speed)
    {
        scrollSpeed = speed;
    }
    void Start()
    {
        AddListPrefabToPoolAndSetParents();
        InitMap();
        GetComponent();
    }

    void Update()
    {
        MoveMap();
        UpdateMap();
        IncreaseScrollSpeed();
    }

    private void GetComponent()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDeath.deathEvent += StopScrollingInTime;
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

    //Dừng khi player chết
    public void StopScrollingInTime()
    {
        StartCoroutine(SmoothStopScroll(durationStop));
    }

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