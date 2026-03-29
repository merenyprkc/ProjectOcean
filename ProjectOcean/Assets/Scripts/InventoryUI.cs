using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public GameObject slotPrefab;
    public Transform slotContainer;
    private InventorySystem inventorySystem;

    [Header("Panel")]
    public GameObject inventoryPanel;

    private InventorySlotUI[] slotUIs;
    private bool isOpen = false;

    private void Start()
    {
        inventorySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        
        CreateSlots();
        inventorySystem.OnInventoryChanged += RefreshUI;

        if (InputManager.Instance != null) InputManager.Instance.OnInventoryToggle += ToggleInventory;

        inventoryPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (inventorySystem != null) inventorySystem.OnInventoryChanged -= RefreshUI;
        if (InputManager.Instance != null) InputManager.Instance.OnInventoryToggle -= ToggleInventory;
    }

    private void CreateSlots()
    {
        slotUIs = new InventorySlotUI[inventorySystem.slotCount];

        for (int i = 0; i < inventorySystem.slotCount; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            slotGO.name = $"Slot {i + 1}";
            slotUIs[i] = slotGO.GetComponent<InventorySlotUI>();
        }
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            InventorySlot slot = inventorySystem.inventorySlots[i];
            slotUIs[i].SetSlot(slot.item, slot.quantity);
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        if (isOpen) InputManager.Instance.DisablePlayerControls();
        else InputManager.Instance.EnablePlayerControls();
    }
}
