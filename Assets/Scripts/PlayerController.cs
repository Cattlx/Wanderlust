using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MoveX, MoveZ;
    public float MovementSpeed = 0.11f;

    Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        rigidBody.MovePosition(transform.position + (transform.forward * MoveZ * MovementSpeed / 2) + (transform.right * MoveX * MovementSpeed / 2));
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.MovePosition(transform.position + (transform.forward * MoveZ * MovementSpeed) + (transform.right * MoveX * MovementSpeed));
            }
        }

    }
}