using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLength = 14;
    [SerializeField]
    private int corridorCount = 5;

    [SerializeField]
    [Range(0.1f,1f)]
    private float roomPercent = .8f;


    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corrides = CreatCorridor(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreatRoom(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnd(floorPositions);

        CreatRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        for(int i = 0;i<corrides.Count;i++)
        {
            //corrides[i] = IncreaseCorridorSizeByOne(corrides[i]);
            corrides[i] = IncreaseCorridorSize3By3(corrides[i]);
            floorPositions.UnionWith(corrides[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreatWalls(floorPositions,tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSize3By3(List<Vector2Int> corridors)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 0; i < corridors.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridors[i] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }
    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFormCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2.zero &&
                directionFormCell != previousDirection)
            {
                for(int x = -1; x < 2; x++)
                {
                    for(int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));                                                                                                    
                    }
                }
                previousDirection = directionFormCell;
                Debug.Log("1");
            }
            else
            {
                Vector2Int newCorridorTileOffset
                    = GetDirection90From(directionFormCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return Vector2Int.right;
        }
        else if(direction == Vector2Int.down)
        {
            return Vector2Int.left;
        }
        else if(direction == Vector2Int.left)
        {
            return Vector2Int.up;
        }
        else if (direction == Vector2Int.right)
        {
            return Vector2Int.down;
        }

        return Vector2Int.zero;
    }

    private void CreatRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        HashSet<Vector2Int> deadEndRoomPositions = new HashSet<Vector2Int>();
        foreach(var position in deadEnds)
        {
            if (roomPositions.Contains(position) == false) 
            {
                var deadEndRoomfloors = RunRandomWalk(randomWalkParameters, position);
                roomPositions.UnionWith(deadEndRoomfloors);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnd(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var position in floorPositions)
        {
            int neighborCount = 0;
            foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighborCount++;
                }
            }
            if(neighborCount == 1) 
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreatRoom(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreatCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);
        List<Vector2Int> roomsToCreat = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreatCount).ToList();

        foreach(var roomPosition in roomsToCreat)
        {
            var roomFloors  = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloors);
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreatCorridor(HashSet<Vector2Int> floorPosition,HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List <List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var cooridor = ProduralGeneration.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(cooridor);
            currentPosition = cooridor[cooridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPosition.UnionWith(cooridor);
        }
        return corridors;
    }
}
