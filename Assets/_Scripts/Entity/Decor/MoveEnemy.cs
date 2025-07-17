using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private bool movingToB = true; // Flag để track đang đi hướng nào

    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1); // Đi về B
        movingToB = true; // Bắt đầu di chuyển về pointB
        target = pointB.position;
    }

    void Update()
    {
        // Di chuyển về target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Đổi target khi ĐÃ ĐẾN target (threshold nhỏ)
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            if (movingToB)
            {
                transform.localScale = new Vector3(1, 1, 1); // Đi về B
                target = pointA.position;
                movingToB = false;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // Đi về A
                target = pointB.position;
                movingToB = true;
            }
        }
    }
}
