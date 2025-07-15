using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Tăng số lượng xu ở đây
            GameManager.Instance.coinIngame++;
            gameObject.SetActive(false);
        }
    }
}
