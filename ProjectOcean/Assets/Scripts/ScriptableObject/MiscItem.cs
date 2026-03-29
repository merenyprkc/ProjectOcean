using UnityEngine;

[CreateAssetMenu(fileName = "MiscItem", menuName = "Survival/New Misc Item")]
public class MiscItem : Item
{
    // bazı şeyler eklenebilir

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Misc;
    }
}