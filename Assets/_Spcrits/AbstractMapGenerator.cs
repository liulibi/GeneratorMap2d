using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GeneratorMap()
    {
        if (tilemapVisualizer == null)
        {
            tilemapVisualizer.ClearTiles();
        }
        RunProceduralGeneration();
    }

    public void ClearMap()
    {
        tilemapVisualizer.ClearTiles();
    }

    protected abstract void RunProceduralGeneration();

}
