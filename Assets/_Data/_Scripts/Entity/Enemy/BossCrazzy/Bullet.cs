using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Di chuyển sang trái theo trục X
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
