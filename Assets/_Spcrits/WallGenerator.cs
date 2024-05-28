using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreatWalls(HashSet<Vector2Int> floorPosition,TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPosition, Direction2D.cardinalDirectionsList);

        foreach (var wallPosition in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(wallPosition);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                if (floorPosition.Contains(position + direction) == false)
                {
                    wallPositions.Add(position+direction);
                }
            }
        }

        return wallPositions;
    }
}
