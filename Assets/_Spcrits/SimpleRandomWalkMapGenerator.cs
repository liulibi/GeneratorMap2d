using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleRandomWalkMapGenerator : AbstractMapGenerator
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk(randomWalkParameters,startPosition);
        tilemapVisualizer.ClearTiles();
        tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreatWalls(floorPosition, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parameters,Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProduralGeneration.SimpleRandomWalk(currentPosition, parameters.walkLength);
            //当前集合中不存在的元素添加到当前集合中
            floorPosition.UnionWith(path); 

            if(parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPosition.ElementAt(UnityEngine.Random.Range(0, floorPosition.Count - 1));
            }
        }
        return floorPosition;
    }

    public void ClearTiles()
    {
        tilemapVisualizer.ClearTiles();
    }
}
