using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Auditory Effects")]
    [SerializeField] private AudioClip _swingAudio;
    [SerializeField] private AudioClip _kickAudio;

    [Header("Attack System")]
    [SerializeField] private float damage = 2f; // TODO: make separate attack class based on weapons
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float attackRadius = 3f; // Radius to affect enemies

    private Animator animator;
    private LayerMask enemyLayer; // Add a layer to filter enemies only

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy"); // Make sure enemies are in the "Enemy" layer
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
        AudioManager.Instance?.PlaySound(_kickAudio);
    }

    private void PerformSwing()
    {
        animator?.SetTrigger("Swing");
        AudioManager.Instance?.PlaySound(_swingAudio);

        // Call the method to damage all enemies within the sphere
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
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        Rigidbody rb = enemy.GetComponent<Rigidbody>();

        health.TakeDamage(damage);

        if (rb != null)
        {
            // Apply knockback in the direction away from the player
            Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

    }

    // Visualization of the attack radius in the Scene view (for debugging purposes)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}