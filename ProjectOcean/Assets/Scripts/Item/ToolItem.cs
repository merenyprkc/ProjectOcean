using UnityEngine;

[CreateAssetMenu(fileName = "ToolItem", menuName = "Survival/New Tool Item")]
public class ToolItem : Item
{
    public float durability;
    public float damage;

    protected override void Reset()
    {
        base.Reset();
        itemType = ItemType.Tool;
    }
}