using UnityEngine;

[CreateAssetMenu(fileName = "MedicineItem", menuName = "Survival/New Medicine Item")]
public class MedicineItem : Item
{
    public float healthRestoreAmount;

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Medicine;
    }
}