using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; // Corrected the alias for UnityEngine.Random

public class SimpleRandomDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField] protected SimpleRandomWalkGeneratorSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk(randomWalkParameters, startPosition); // Added startPosition as an argument
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
    }

    protected static HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkGeneratorSO parameters, Vector2Int position) // Added static keyword
    {
        var currentPosition = position; // Corrected typo: currentPositon to currentPosition
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength); // Changed currentPositon to currentPosition
            floorPosition.UnionWith(path);

            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }

}
