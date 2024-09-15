using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    bool isInventoryActive;

    private void Start()
    {
        SetInventoryActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (isInventoryActive)
        {
            SetInventoryActive(false);
        }
        else
        {
            SetInventoryActive(true);
            InventorySystem.current.DisplayInventory();
        }
    }

    private void SetInventoryActive(bool active)
    {
        inventory.SetActive(active);
        isInventoryActive = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
