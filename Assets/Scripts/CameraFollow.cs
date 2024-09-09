using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow; // The object the camera should follow
    Vector3 cameraOffset = new Vector3(0.42f, 1.69f, -1.22f); // Offset of the camera from the object

    void Update()
    {
        // Set the camera's position to the object's position plus the offset, without modifying the object's position
        transform.position = objectToFollow.transform.position + cameraOffset;
    }
}