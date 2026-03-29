using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public Item item;
    public int quantity;

    public void Interact(GameObject interactor = null)
    {
        InventorySystem inv = interactor.GetComponent<InventorySystem>();
        int added = inv.AddItem(item, quantity);
        quantity -= added;

        if (quantity <= 0) Destroy(gameObject);
    }
}
