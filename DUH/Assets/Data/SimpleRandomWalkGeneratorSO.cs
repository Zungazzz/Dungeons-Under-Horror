using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkGenerator", menuName = "PCG/SimpleRandomWalkData")]

public class SimpleRandomWalkGeneratorSO : ScriptableObject
{
    public int iterations = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;
}
