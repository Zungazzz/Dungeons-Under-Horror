using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPassiveItems : PassiveItems
{
    protected override void ApplyModifiers()
    {
        playerStats.CurrentMoveSpeed += 1 + passiveItemsScriptableObject.Multiply / 100f;
    }
}