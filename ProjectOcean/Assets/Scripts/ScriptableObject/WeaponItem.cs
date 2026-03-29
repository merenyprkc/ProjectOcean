using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Survival/New Weapon Item")]
public class WeaponItem : Item
{
    public float damage;
    public float durability;

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Weapon;
    }
}