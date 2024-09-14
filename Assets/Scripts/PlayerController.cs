using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 1.33f;
    [SerializeField] float sprintSpeed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] LayerMask GroundLayer;

    float MoveX, MoveZ;
    float RotationSpeed = 2f;
    float GroundCheckDistance = -0.8f;
    Vector3 velocity;
    bool isGrounded;

    CharacterController controller;
    Animator animator;


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



    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 + GroundCheckDistance, GroundLayer);
    }
}
