using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.33f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask GroundLayer;

    private float MoveX, MoveZ;
    private float RotationSpeed = 2f;
    private float GroundCheckDistance = 0.1f;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Attack System")]
    [SerializeField] private float damage = 2f; //TODO: make separate attack class based on weapons

    [Header("Auditory Effects")]
    [SerializeField] private AudioClip _swingAudio;
    [SerializeField] private AudioClip _kickAudio;
    [SerializeField] private AudioClip _pickupAudio;

    private CharacterController controller;
    private Animator animator;

    private GameObject currentItemToPickup;
    private GameObject enemyToDamage;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovementInput();
        HandleRotation();
        HandleInteractions();
    }

    void FixedUpdate()
    {
        GroundCheck();
        ApplyGravity();
    }

    private void HandleMovementInput()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        bool isMoving = movementDirection.sqrMagnitude > 0.01f;

        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            Vector3 move = movementDirection * currentSpeed * Time.deltaTime;
            controller.Move(move);
        }
    }

    private void HandleRotation()
    {
        float rotation = RotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation, 0);
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to ensure it's grounded
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void HandleInteractions()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Kick");
            AudioManager.Instance.PlaySound(_kickAudio);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Swing");
            AudioManager.Instance.PlaySound(_swingAudio);
            if (enemyToDamage != null)
            {
                DamageEnemy(enemyToDamage);
            }
        }

        if (currentItemToPickup != null && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem(currentItemToPickup);
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 + GroundCheckDistance, GroundLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemObject item))
        {
            currentItemToPickup = item.gameObject;
        }

        if (other.CompareTag("Enemy"))
        {
            enemyToDamage = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItemToPickup)
        {
            currentItemToPickup = null;
        }

        if (other.CompareTag("Enemy"))
        {
            enemyToDamage = null;
        }
    }

    private void PickupItem(GameObject item)
    {
        if (item.TryGetComponent(out ItemObject itemObj))
        {
            AudioManager.Instance.PlaySound(_pickupAudio);
            itemObj.OnHandlePickupItem();
            Destroy(item);
        }
    }

    private void DamageEnemy(GameObject enemy)
    {
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
