using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public ItemType.ItemTypeEnum itemType;  // Correct reference to enum
    public Sprite icon;
    public GameObject prefab;
}