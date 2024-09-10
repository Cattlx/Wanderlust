using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow; // The object the camera should follow
    public Vector3 cameraOffset = new Vector3(0, 1.69f, -2.0f); // Offset of the camera from the object

    void Update()
    {
        // Calculate the target rotation based on the object's rotation and the desired offset
        Quaternion targetRotation = objectToFollow.transform.rotation * Quaternion.Euler(cameraOffset);

        // Set the camera's rotation directly to the target rotation
        transform.rotation = targetRotation;

        // Set the camera's position to the object's position plus the offset
        transform.position = objectToFollow.transform.position + targetRotation * cameraOffset;
    }
}