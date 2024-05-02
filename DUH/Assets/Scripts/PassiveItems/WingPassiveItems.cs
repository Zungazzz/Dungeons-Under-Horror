using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPassiveItems : PassiveItems
{
    protected override void ApplyModifiers()
    {
        playerStats.currentMoveSpeed += 1 + passiveItemsScriptableObject.Multiply / 100f;
    }
}