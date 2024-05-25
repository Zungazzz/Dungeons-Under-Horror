using UnityEngine;

public class MagnetModifier : PassiveItems
{
    protected override void ApplyModifiers()
    {
        playerStats.CurrentMagnet += (float)1 + passiveItemsScriptableObject.Multiply / 100f;
    }

}
