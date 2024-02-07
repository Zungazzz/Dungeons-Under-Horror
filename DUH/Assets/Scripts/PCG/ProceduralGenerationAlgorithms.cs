using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousposition = startPosition;
        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousposition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousposition = newPosition;
        }
        return path;
    }
     //using list instead of hashset to keep track of previous positions
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength){
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPositon = startPosition;
        corridor.Add(currentPositon);
        for (int i = 0; i < corridorLength; i++)
        {
            currentPositon += direction;
            corridor.Add(currentPositon);
        }
        return corridor;
    }
        public static List<BoundsInt> BinarySpacePartitioning(BoundInt spaceToSplit , int minWidth, int minHeight){
            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
            List<BoundsInt> roomsList = new List<BoundsInt>();
            roomsQueue.Enqueue(spaceToSplit);
        }
    }
   
    public static class Direction2D
    {
        public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
        {
            new Vector2Int(1, 0), // RIGHT
            new Vector2Int(-1, 0), // LEFT
            new Vector2Int(0, 1), // UP
            new Vector2Int(0, -1) // DOWN
        };

        public static Vector2Int GetRandomCardinalDirection()
        {
            return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
        }
        
    }
