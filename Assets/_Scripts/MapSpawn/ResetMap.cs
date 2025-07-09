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

    private void StoreOriginalPositions()
    {
        originalPositions.Clear();
        foreach (GameObject obj in objList)
        {
            if (obj == null) continue;
            foreach (Transform child in obj.transform)
            {
                if (child != null)
                {
                    originalPositions[child] = child.localPosition;
                }
            }
        }
    }

    public void _ResetMap()
    {
        foreach (GameObject obj in objList)
        {
            if (obj == null) continue;
            foreach (Transform child in obj.transform)
            {
                if (child != null && originalPositions.TryGetValue(child, out Vector3 pos))
                {
                    child.localPosition = pos;
                }
                child.gameObject.SetActive(true);
            }
        }
    }

    public void SpawnPowerUp()
    {
        if (isSpawned) return;
        isSpawned = true;
        ResetPowerUp();

        int randomIndexSpawnPoint = Random.Range(0, spawnPointList.childCount);
        int randomIndexPowerUp = Random.Range(0, powerUpList.childCount);

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
