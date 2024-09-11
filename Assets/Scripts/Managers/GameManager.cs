using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public Health playerHealth;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update Player Health UI
        // TODO: update this only if health changed cuz performance
        UIManager.UpdateHealth(playerHealth.health / playerHealth.maxHealth);
    }
}
