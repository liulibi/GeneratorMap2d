using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private Tilemap wallTilemap;

    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTop;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> floorPositions, Tilemap floorTilemap, TileBase floorTile)
    {
        foreach (var position in floorPositions)
        {
            PaintSingleTile(position, floorTilemap, floorTile);
        }
    }

    private void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition,tile);
    }

    public void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(position, wallTilemap, wallTop);
    }

    public void ClearFloorTiles()
    {
        floorTilemap.ClearAllTiles();
    }

    public void ClearWallTiles()
    {
        wallTilemap.ClearAllTiles();
    }

    public void ClearTiles()
    {
        ClearFloorTiles();
        ClearWallTiles();
    }
}


