using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attack Settings")]
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked;

    [Header("Damage Settings")]
    public float attackDamage;
    public float attackRange;

    [Header("States")]
    public float sightRange;
    bool playerInSightRange, playerInAttackRange;

    [Header("Misc")]
    public NavMeshAgent agent;
    public LayerMask groundMask, playerMask;
    Transform player;

    private Animator animator;
    private Health playerHealth;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        playerHealth = player.GetComponent<Health>();
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("IsWalking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in walk range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, .1f, groundMask))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("IsWalking", true);
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        animator.SetBool("IsWalking", false);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Swing");

            // Check if player is in range to take damage
            if (playerInAttackRange)
            {
                DealDamageToPlayer();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void DealDamageToPlayer()
    {
        playerHealth.TakeDamage(attackDamage);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}