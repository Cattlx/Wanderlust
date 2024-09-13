using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject inventory;
    bool isInventoryActive;

    void Start()
    {
        inventory.SetActive(false);
        isInventoryActive = false;
    }

    public void UpdateHealth(float healthPercent)
    {
        // Assuming healthPercent is a value between 0 and 1 (normalized)
        healthSlider.value = healthPercent;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryActive)
        {
            RenderInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isInventoryActive)
        {
            CloseInventory();
        }
    }

    private void RenderInventory()
    {
        inventory.SetActive(true);
        isInventoryActive = true;
        Cursor.lockState = CursorLockMode.None;
        InventorySystem.current.DisplayInventory();
    }

    private void CloseInventory()
    {
        inventory.SetActive(false);
        isInventoryActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
