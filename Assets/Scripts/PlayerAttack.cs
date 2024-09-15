using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{

    [Header("Attack System")]
    [SerializeField] private float damage = 2f; // TODO: make separate attack class based on weapons
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float attackRadius = 3f;

    private Animator animator;
    private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PerformKick();
        }

        if (Input.GetMouseButtonDown(0))
        {
            PerformSwing();
        }
    }

    private void PerformKick()
    {
        animator?.SetTrigger("Kick");
    }

    private void PerformSwing()
    {
        animator?.SetTrigger("Swing");

        ApplyDamageAndKnockback();
    }

    private void ApplyDamageAndKnockback()
    {
        // Find all colliders within the attack radius that are on the "Enemy" layer
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (Collider enemyCollider in hitEnemies)
        {
            GameObject enemy = enemyCollider.gameObject;
            DamageEnemy(enemy);
        }
    }

    private void DamageEnemy(GameObject enemy)
    {
        Health health = enemy.GetComponent<Health>();
        Rigidbody rb = enemy.GetComponent<Rigidbody>();

        health.TakeDamage(damage);

        if (rb != null)
        {
            // Apply knockback in the direction away from the player
            Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}