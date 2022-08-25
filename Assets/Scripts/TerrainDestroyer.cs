using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    public Tilemap terrain;
    public void DestroyTerrain(Vector3 hitPos)
    {
        Vector3Int tilePos = terrain.WorldToCell(hitPos);
        if (terrain.GetTile(tilePos) != null)
        {
            Debug.Log("tile destroyed pos: "+tilePos);
            DestroyTile(tilePos);
        }
         
    }

    private void DestroyTile(Vector3Int tilePos)
    {
        terrain.SetTile(tilePos, null);
    }
}
