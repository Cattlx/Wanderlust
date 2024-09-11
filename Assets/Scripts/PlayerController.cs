using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 1.33f;
    public float RotationSpeed = 2f;
    public float Gravity = -9.81f;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Capture input for movement and rotation
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        // Movement direction based on input
        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        bool isMoving = movementDirection.magnitude > 0.1f; // Adjust threshold as needed

        // Set animator trigger for walking
        animator.SetBool("IsWalking", isMoving);

        // Handle kick and swing animations
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Kick");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Swing");
        }

        // Rotate the character based on mouse input
        float rotation = RotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation, 0);

        // Apply movement
        if (isMoving)
        {
            characterController.Move(movementDirection * MovementSpeed * Time.deltaTime);
        }

        // Apply gravity
        if (characterController.isGrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += Gravity * Time.deltaTime;
        }

        // Apply gravity effect through CharacterController
        characterController.Move(velocity * Time.deltaTime);
    }
}
