using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class MatchTilemapSize : MonoBehaviour
{
    public Tilemap tilemap; // Reference to your Tilemap

    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference not set in the inspector.");
            return;
        }

        MatchCameraSizeToTilemap();
        CenterCameraOnTilemap();
    }

    void MatchCameraSizeToTilemap()
    {
        // Get the dimensions of the Tilemap
        Vector3 tilemapSize = tilemap.size;

        // Calculate the orthographic size based on the height (you may use width for a different aspect ratio)
        float orthoSize = tilemapSize.y / 2;

        // Set the camera's orthographic size
        Camera.main.orthographicSize = orthoSize;
    }

    void CenterCameraOnTilemap()
    {
        // Get the center of the Tilemap
        Vector3 tilemapCenter = tilemap.transform.position + new Vector3(tilemap.size.x / 2, tilemap.size.y / 2, 0);

        // Set the camera's position
        Camera.main.transform.position = new Vector3(tilemapCenter.x, tilemapCenter.y, Camera.main.transform.position.z);
    }
}
