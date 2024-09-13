using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 1.33f;
    public float SprintSpeed = 5f;
    public float RotationSpeed = 2f;
    public float Gravity = -9.81f; // Earth's gravity in Unity units per second squared
    public float GroundCheckDistance = 0.1f; // Distance to check if the player is grounded
    public LayerMask GroundLayer;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity; // Store the player's vertical velocity
    private bool isGrounded;

    private GameObject currentItemToPickup; // Reference to the item in trigger zone

    [Header("Auditory Effects")]
    [SerializeField] private AudioClip _swingAudio;
    [SerializeField] private AudioClip _kickAudio;
    [SerializeField] private AudioClip _pickupAudio; // Audio for pickup

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Capture input for movement and rotation
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        // Check if the player is grounded
        GroundCheck();

        // Reset velocity.y if the player is grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep the player grounded
        }

        // Movement direction based on input
        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        bool isMoving = movementDirection.magnitude > 0.1f; // Adjust threshold as needed

        // Set animator trigger for walking
        animator.SetBool("IsWalking", isMoving);

        // Handle kick and swing animations
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Kick");
            AudioManager.Instance.PlaySound(_kickAudio);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Swing");
            AudioManager.Instance.PlaySound(_swingAudio);
        }

        // Rotate the character based on mouse input
        float rotation = RotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation, 0);

        // Apply movement
        if (isMoving)
        {
            float currentSpeed = MovementSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = SprintSpeed; // Sprint by multiplying speed by 2
            }

            Vector3 move = movementDirection * currentSpeed;
            controller.Move(move * Time.deltaTime); // Move the character controller
        }

        // Apply gravity
        velocity.y += Gravity * Time.deltaTime; // Add gravity to the vertical velocity
        controller.Move(velocity * Time.deltaTime); // Apply vertical movement (gravity)

        // Check for "E" key press to pick up item
        if (currentItemToPickup != null && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem(currentItemToPickup);
        }
    }

    // Ground check using a simple raycast
    void GroundCheck()
    {
        RaycastHit hit;

        // Raycast slightly below the player to check if grounded
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 + GroundCheckDistance, GroundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Detect when player enters a trigger (pickup zone)
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a tag or component indicating it's a pickup item
        if (other.TryGetComponent<ItemObject>(out ItemObject item))
        {
            currentItemToPickup = item.gameObject;
            Debug.Log("Item in range for pickup: " + currentItemToPickup.name);
        }
    }

    // Detect when player exits a trigger (pickup zone)
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItemToPickup)
        {
            currentItemToPickup = null; // No longer in range to pick up item
            Debug.Log("Item out of range for pickup.");
        }
    }

    // Pickup logic
    private void PickupItem(GameObject item)
    {
        if (item.TryGetComponent<ItemObject>(out ItemObject itemObj))
        {
            AudioManager.Instance.PlaySound(_pickupAudio);
            itemObj.OnHandlePickupItem();
            Destroy(item);
        }
    }

}
