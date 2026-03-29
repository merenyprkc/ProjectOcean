using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Item : ScriptableObject
{
    [Header("Item Properties")]
    public int itemID;
    public string itemName;
    [TextArea] public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public float itemValue;
    public bool isStackable = true;
    public int itemStackLimit = 16;

    [Header("Prefabs")]
    public GameObject worldItemPrefab;
    public GameObject handItemPrefab;

    #if UNITY_EDITOR
    protected virtual void Reset()
    {
        itemID = CreateNewID();
        if(!isStackable) itemStackLimit = 1;
    }

    private int CreateNewID()
    {
        string[] guids = AssetDatabase.FindAssets("t:Item");
        int maxID = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
            if (item != null && item.itemID > maxID)
            {
                maxID = item.itemID;
            }
        }
        return maxID + 1;
    }
    #endif
}

public enum ItemType
{
    None,          // Hiçbir tür atanmazsa varsayılan.
    Consumable,    // Elma, Armut, Su, vb.
    Medicine,      // Bandaj, İlaç, vb.
    Tool,          // Balta, Kazma, vb.
    Weapon,        // Tahta Kılıç, Taş Kılıç, Demir Kılıç, vb.
    Clothing,      // T-Shirt, Pantolon, Ceket, vb.
    Misc           // Hammadde, Malzeme, vb.
}
