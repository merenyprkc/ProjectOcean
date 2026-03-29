using UnityEngine;

[CreateAssetMenu(fileName = "ClothingItem", menuName = "Survival/New Clothing Item")]
public class ClothingItem : Item
{
    public float protection;
    public float durability;

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Clothing;
    }
}