using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;

    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    private List<InventoryItem> inventory;

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryUIPanel;
    [SerializeField] private GameObject itemUIPrefab;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        itemDictionary.TryGetValue(referenceData, out InventoryItem item);
        return item;
    }

    public void Add(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem item))
        {
            item.AddToStack();
        }
        else
        {
            item = new InventoryItem(referenceData);
            inventory.Add(item);
            itemDictionary[referenceData] = item;
        }

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        // Clear existing UI items
        foreach (Transform child in inventoryUIPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate UI with current inventory
        foreach (InventoryItem item in inventory)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, inventoryUIPanel.transform);
            Image itemIconImage = itemUI.GetComponent<Image>();
            itemIconImage.sprite = item.data.icon;
        }
    }
}
