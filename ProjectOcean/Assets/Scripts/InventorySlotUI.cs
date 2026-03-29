using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;

    public void SetSlot(Item item, int quantity)
    {
        bool hasItem = item != null;

        icon.sprite = hasItem ? item.itemIcon : null;
        icon.enabled = hasItem;
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
    }
}
