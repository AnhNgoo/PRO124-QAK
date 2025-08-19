using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMap : MonoBehaviour
{
    public List<GameObject> objList = new();

    public Transform spawnPointList;
    public Transform powerUpList;

    private bool isSpawned = false;
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

    private void Awake()
    {
        StoreOriginalPositions();
    }

    //Lưu toàn bộ vị trí gốc của các đối tượng con trong objList
    private void StoreOriginalPositions()
    {
        originalPositions.Clear();

        foreach (GameObject obj in objList) //lấy danh sách các đối tượng: Coin, PowerUp, ...
        {
            if (obj == null) continue;
            foreach (Transform child in obj.transform) //lấy danh sách các đối tượng con của Coin, PowerUp, ... và lưu nó vào originalPositions và lấy key là child đã gán
            {
                if (child != null)
                {
                    originalPositions[child] = child.localPosition;
                }
            }
        }
    }

    //Đặt lại vị trí của các đối tượng con về vị trí gốc đã lưu
    public void _ResetMap()
    {
        foreach (GameObject obj in objList) //lấy danh sách các đối tượng: Coin, PowerUp, ...
        {
            if (obj == null) continue;
            foreach (Transform child in obj.transform) //lấy danh sách các đối tượng con của Coin, PowerUp, ... 
            {
                if (child != null && originalPositions.TryGetValue(child, out Vector3 pos)) // tìm vị trí gốc của child bằng key (child) và trả về vị trí đã lưu
                {
                    child.localPosition = pos;
                }
                child.gameObject.SetActive(true);
            }
        }
    }

    //Spawn PowerUp tại vị trí ngẫu nhiên trong danh sách spawnPointList
    public void SpawnPowerUp()
    {
        if (isSpawned) return;
        isSpawned = true;
        ResetPowerUp();

        int randomIndexSpawnPoint = Random.Range(0, spawnPointList.childCount); // Random index của vị trí spawn
        int randomIndexPowerUp = Random.Range(0, powerUpList.childCount);  //Random index powerUp sẽ xuất hiện

        Transform spawnPoint = spawnPointList.GetChild(randomIndexSpawnPoint);
        Transform powerUp = powerUpList.GetChild(randomIndexPowerUp);

        powerUp.position = spawnPoint.position;
        powerUp.gameObject.SetActive(true);

    }

    private void OnEnable()
    {
        isSpawned = false;
        ResetPowerUp();
        _ResetMap();
    }

    private void ResetPowerUp()
    {
        foreach (Transform child in powerUpList)
        {
            child.gameObject.SetActive(false);
        }
    }
}
