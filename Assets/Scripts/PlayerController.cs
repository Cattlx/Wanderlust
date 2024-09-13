using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 1.33f;
    public float SprintSpeed = 5f;
    public float RotationSpeed = 2f;
    public float Gravity = -9.81f;
    public float GroundCheckDistance = 0.1f;
    public LayerMask GroundLayer;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    private GameObject currentItemToPickup;
    private GameObject enemyToDamage;

    public float playerDamage = 2f;

    [Header("Auditory Effects")]
    [SerializeField] private AudioClip _swingAudio;
    [SerializeField] private AudioClip _kickAudio;
    [SerializeField] private AudioClip _pickupAudio;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovementInput();
        GroundCheck();
        ApplyGravity();
        HandleInteractions();
        HandleRotation();
    }

    private void HandleMovementInput()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        bool isMoving = movementDirection.magnitude > 0.1f;

        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : MovementSpeed;
            controller.Move(movementDirection * currentSpeed * Time.deltaTime);
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
            velocity.y = -2f;
        }

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
        if (other.TryGetComponent<ItemObject>(out ItemObject item))
        {
            currentItemToPickup = item.gameObject;
            Debug.Log("Item in range for pickup: " + currentItemToPickup.name);
        }

        if (other.CompareTag("Enemy"))
        {
            enemyToDamage = other.gameObject;
            Debug.Log("Enemy in range for damage");
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
        if (item.TryGetComponent<ItemObject>(out ItemObject itemObj))
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
            health.TakeDamage(playerDamage);
        }
    }
}
