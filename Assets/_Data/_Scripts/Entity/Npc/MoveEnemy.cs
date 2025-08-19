using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = pointB.localPosition;
        transform.localScale = new Vector3(-1, 1, 1); // xoay về B
    }

    void Update()
    {
        // di chuyển trong local space
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.localPosition, target) < 0.05f)
        {
            if (target == pointB.localPosition)
            {
                target = pointA.localPosition;
                transform.localScale = new Vector3(1, 1, 1); // xoay về A
            }
            else
            {
                target = pointB.localPosition;
                transform.localScale = new Vector3(-1, 1, 1); // xoay về B
            }
        }
    }
}
