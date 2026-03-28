using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Survival/New Consumable Item")]
public class ConsumableItem : Item
{
    public float healthRestoreAmount;
    public float staminaRestoreAmount;
    public float hungerRestoreAmount;
    public float thirstRestoreAmount;

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Consumable;
    }
}