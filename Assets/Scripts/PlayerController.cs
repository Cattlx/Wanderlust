using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 1.33f;
    [SerializeField] float sprintSpeed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] LayerMask groundLayer;

    float moveX, moveZ;
    Vector3 velocity;
    bool isGrounded;

    CharacterController controller;
    Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ApplyGravity();
    }

    private void HandleMovementInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 movementDirection = (transform.forward * moveZ + transform.right * moveX).normalized;
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
        float rotation = rotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * rotation);
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to ensure it's grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + groundCheckDistance, groundLayer);
    }
}
