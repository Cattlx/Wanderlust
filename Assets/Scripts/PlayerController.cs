using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    float MovementSpeed = 15;
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
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        Vector3 movementDirection = (transform.forward * MoveZ + transform.right * MoveX).normalized;

        // Check if player is moving
        bool isMoving = movementDirection.magnitude > 0.1f; // Adjust threshold as needed

        // Set animator trigger
        animator.SetBool("IsWalking", isMoving);

        // Apply movement
        rigidBody.MovePosition(transform.position + movementDirection * MovementSpeed * Time.deltaTime);

        // Rotate player
        float rotation = RotationSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation, 0);
    }
}