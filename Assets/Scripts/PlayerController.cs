using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 1.33f;
    float RotationSpeed = 2f;

    private Rigidbody rigidBody;
    private Animator animator;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Capture input for movement and rotation
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        // Check if player is moving
        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        bool isMoving = movementDirection.magnitude > 0.1f; // Adjust threshold as needed

        // Set animator trigger
        animator.SetBool("IsWalking", isMoving);

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Kick");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Swing");
        }


        float rotation = RotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation, 0);
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for consistent physics behavior
        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;
        rigidBody.MovePosition(transform.position + movementDirection * MovementSpeed * Time.fixedDeltaTime);
    }
}
