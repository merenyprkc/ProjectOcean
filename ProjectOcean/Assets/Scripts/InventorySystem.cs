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
        for(int i = 0; i < slotCount; i++)
        {
            inventorySlots.Add(new InventorySlot(null, 0));
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        if(item == null || quantity <= 0)
        {
            Debug.LogWarning("Invalid item or quantity.");
            return false;
        }

        if (item.isStackable)
        {
            InventorySlot existingSlot = inventorySlots.Find(slot => slot.item == item);
            if (existingSlot != null)
            {
                int totalQuantity = existingSlot.quantity + quantity;
                if (totalQuantity <= item.itemStackLimit)
                {
                    existingSlot.quantity = totalQuantity;
                    OnInventoryChanged?.Invoke();
                    return true;
                }
                else
                {
                    int remainingQuantity = totalQuantity - item.itemStackLimit;
                    existingSlot.quantity = item.itemStackLimit;
                    quantity = remainingQuantity;
                }
            }
        }

        while (quantity > 0)
        {
            if(inventorySlots.Count >= slotCount)
            {
                Debug.LogWarning("Inventory is full. Cannot add more items.");
                return false;
            }

            int addQuantity = item.isStackable ? Mathf.Min(quantity, item.itemStackLimit) : 1;
            inventorySlots.Add(new InventorySlot(item, addQuantity));
            quantity -= addQuantity;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}
