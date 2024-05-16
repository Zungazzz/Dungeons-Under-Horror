using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPassiveItems : PassiveItems
{
    protected override void ApplyModifiers()
    {
        playerStats.CurrentMight += 1 + passiveItemsScriptableObject.Multiply / 100f;
    }
}
