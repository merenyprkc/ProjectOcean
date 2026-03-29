using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }    
}

public class InventorySystem : MonoBehaviour
{
    [Header("Settings")]
    public int slotCount = 20;

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public event Action OnInventoryChanged;

    private void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            inventorySlots.Add(new InventorySlot(null, 0));
        }

        OnInventoryChanged?.Invoke();
    }

    public int AddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogWarning("Invalid item or quantity.");
            return 0;
        }

        int originalQuantity = quantity;

        if (item.isStackable)
        {
            InventorySlot existingSlot = inventorySlots.Find(slot => slot.item == item);
            if (existingSlot != null)
            {
                int canAdd = item.itemStackLimit - existingSlot.quantity;
                if (canAdd > 0)
                {
                    int toAdd = Mathf.Min(quantity, canAdd);
                    existingSlot.quantity += toAdd;
                    quantity -= toAdd;
                }

                if (quantity <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return originalQuantity;
                }
            }
        }

        while (quantity > 0)
        {
            InventorySlot emptySlot = inventorySlots.Find(slot => slot.item == null);
            if (emptySlot == null)
            {
                Debug.LogWarning("Inventory is full. Cannot add more items.");
                break;
            }

            int addQuantity = item.isStackable ? Mathf.Min(quantity, item.itemStackLimit) : 1;
            emptySlot.item = item;
            emptySlot.quantity = addQuantity;
            quantity -= addQuantity;
        }

        int added = originalQuantity - quantity;
        if (added > 0)
            OnInventoryChanged?.Invoke();

        return added;
    }
}
