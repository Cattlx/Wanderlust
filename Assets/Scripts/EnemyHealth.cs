using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 20f;
    public float maxHealth = 20f;


    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}