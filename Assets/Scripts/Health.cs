using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 20f;
    public float maxHealth = 20f;

    float chachedHealth;

    public GameManager gameManager;

    void Start()
    {
        chachedHealth = health;
    }

    void Update()
    {

        if (chachedHealth != health)
        {
            gameManager.UpdateUI();
            chachedHealth = health;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}