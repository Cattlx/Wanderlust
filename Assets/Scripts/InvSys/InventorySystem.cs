using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }
    public static InventorySystem current;

    // Reference to UI elements
    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryUIPanel;  // The panel or scroll view where items will be displayed
    [SerializeField] private GameObject itemUIPrefab;      // Prefab for displaying an item in the inventory (Text/Image)

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        current = this;
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        DisplayInventory();  // Update the UI whenever an item is added
    }

    // Method to display all items in the UI
    public void DisplayInventory()
    {
        // Clear the current UI items first
        foreach (Transform child in inventoryUIPanel.transform)
        {
            Destroy(child.gameObject); // Clean up previous UI elements
        }

        // Populate UI with inventory items
        foreach (InventoryItem item in inventory)
        {
            // Instantiate a new UI item for each inventory item
            GameObject itemUI = Instantiate(itemUIPrefab, inventoryUIPanel.transform);

            // Assuming itemUIPrefab has Text and Image components to display name and icon
            Text itemNameText = itemUI.transform.Find("ItemNameText").GetComponent<Text>();
            Image itemIconImage = itemUI.transform.Find("ItemIconImage").GetComponent<Image>();

            // Set the text and image based on the item's data
            itemNameText.text = item.data.displayName;
            itemIconImage.sprite = item.data.icon;
        }
    }

}
