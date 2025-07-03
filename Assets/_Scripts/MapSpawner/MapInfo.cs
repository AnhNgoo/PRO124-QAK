using UnityEngine;
using UnityEngine.Tilemaps;

public class MapInfo : MonoBehaviour
{
    private Tilemap tilemap;
    private float mapLength = 10f;

    public float MapLength => mapLength;

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap != null)
        {
            int tileCountX = tilemap.cellBounds.size.x;
            float tileSizeX = tilemap.layoutGrid.cellSize.x;
            float scaleX = tilemap.transform.lossyScale.x;

            mapLength = tileCountX * tileSizeX * scaleX;
        }
        else
        {
            Debug.LogWarning($"[{name}] Không tìm thấy Tilemap.");
        }
    }
}
