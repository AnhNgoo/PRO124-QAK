using UnityEngine;

public class OriginPosition : MonoBehaviour
{
    private Vector3 originPosition;

    void OnEnable()
    {
        originPosition = transform.position; // Lưu vị trí ban đầu
    }

    public void ResetPosition()
    {
        transform.position = originPosition;
    }
}
