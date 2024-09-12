using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 1.33f;
    public float RotationSpeed = 2f;

    private CharacterController controller;
    private Animator animator;

    // LayerMask for what the player considers as ground
    public LayerMask GroundLayer;

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
            Vector3 move = movementDirection * MovementSpeed;
            controller.Move(move * Time.deltaTime); // Move the character controller
        }
    }
}
