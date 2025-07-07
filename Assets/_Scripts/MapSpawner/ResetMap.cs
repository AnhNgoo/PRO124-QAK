using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMap : MonoBehaviour
{
    public List<GameObject> gameobjectList = new List<GameObject>();
    public void Reset()
    {
        foreach (GameObject obj in gameobjectList)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
