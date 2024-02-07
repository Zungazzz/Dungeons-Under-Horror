using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorFirstDungeonGenerator : SimpleRandomDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 3;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = .8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

private void CorridorFirstGeneration()
{
    HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
    HashSet<Vector2Int> potentialRoomPosition = new HashSet<Vector2Int>();
    List<List<Vector2Int>> corridors = CreateCorridors(floorPosition, potentialRoomPosition);

    HashSet<Vector2Int> roomPosition = CreateRooms(potentialRoomPosition);

    List<Vector2Int> deadEnds = FindAllDeadEnds(floorPosition);
    CreateRoomsAtDeadEnds(deadEnds, roomPosition);
    floorPosition.UnionWith(roomPosition);

    for (int i = 0; i < corridors.Count; i++)
    {
        corridors[i] = IncreaseCorridorBrush3By3(corridors[i]);
        floorPosition.UnionWith(corridors[i]);
    }

    tilemapVisualizer.PaintFloorTiles(floorPosition);
    WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
}


    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPosition)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPosition)
        {
            int neighbourCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPosition.Contains(position + direction))
                {
                    neighbourCount++;
                }
            }
            if (neighbourCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPosition)
    {
        foreach (var position in deadEnds)
        {
            if (!roomPosition.Contains(position))
            {
                var roomFloor = RunRandomWalk(randomWalkParameters, position);
                roomPosition.UnionWith(roomFloor);
            }
        }
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = (int)(potentialRoomPosition.Count * roomPercent);
        List<Vector2Int> roomsToCreate = potentialRoomPosition.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();  
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPosition, HashSet<Vector2Int> potentialRoomPosition)
    {
        var currentPosition = startPosition;
        potentialRoomPosition.Add(currentPosition);

        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPosition.Add(currentPosition);
            floorPosition.UnionWith(corridor);
        }
        return corridors;
    }

public List<Vector2Int> IncreaseCorridorBrush3By3(List<Vector2Int> corridor)
{
    List<Vector2Int> newCorridor = new List<Vector2Int>();
    foreach (var cell in corridor)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                newCorridor.Add(cell + new Vector2Int(x, y));
            }
        }
    }
    return newCorridor;
}


    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previewDirection = Vector2Int.zero;
        for (int i = 0; i < corridor.Count; i++)
        {
            if (i > 0)
            {
                Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
                if (previewDirection != Vector2Int.zero && directionFromCell != previewDirection)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                        }
                    }
                    previewDirection = directionFromCell;
                }
                else
                {
                    Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                    newCorridor.Add(corridor[i - 1]);
                    newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
                }
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return Vector2Int.left;
        }
        if (direction == Vector2Int.down)
        {
            return Vector2Int.right;
        }
        if (direction == Vector2Int.left)
        {
            return Vector2Int.down;
        }
        if (direction == Vector2Int.right)
        {
            return Vector2Int.up;
        }
        return Vector2Int.zero;
    }
}
