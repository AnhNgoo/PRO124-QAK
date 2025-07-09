using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    public List<GameObject> rockets = new List<GameObject>();


    void OnEnable()
    {
        DisableRockets();
        StartCoroutine(RandomRockets());
    }

    private void DisableRockets()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    IEnumerator RandomRockets()
    {
        //Random số lượng tên lửa sẽ spawn
        int randomIndex = Random.Range(0, rockets.Count - 1);


        for (int i = 0; i <= randomIndex; i++)
        {

            int randomRocketIndex = Random.Range(0, rockets.Count);
            rockets[randomRocketIndex].SetActive(true);

            // Đợi đến khi rocket tắt lại (RocketMovement sẽ tắt chính nó)
            while (rockets[randomRocketIndex].activeSelf)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.5f);
        DisableThisGameObject();
    }

    private void DisableThisGameObject()
    {
        gameObject.SetActive(false);
    }
}