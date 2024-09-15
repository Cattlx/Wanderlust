using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public ItemType.ItemTypeEnum itemType;
    public Sprite icon;
    public GameObject prefab;
}