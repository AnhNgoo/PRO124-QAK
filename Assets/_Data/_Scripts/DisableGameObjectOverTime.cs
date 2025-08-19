using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOverTime : MonoBehaviour
{
    public float disableTime = 5f; // Thời gian sau đó GameObject sẽ bị vô hiệu hó
    private void OnEnable()
    {
        StartCoroutine(StartDisableCountdown());
    }

    IEnumerator StartDisableCountdown()
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false); // Vô hiệu hóa GameObject sau thời gian đã định
    }
}

//Tắt gameobject theo thời gian tuỳ chỉnh