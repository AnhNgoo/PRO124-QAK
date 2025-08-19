using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public LayerMask groundLayer;
    public float checkDistance = 0.1f;

    public bool Check()
    {
        // Kiểm tra xem có chạm đất hay không
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        // Nếu có chạm đất, trả về true
        if (hit.collider != null)
        {
            return true;
        }

        // Nếu không chạm đất, trả về false
        return false;
    }
}
