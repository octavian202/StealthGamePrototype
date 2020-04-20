using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offview = 0.5f;

    public float smoothSpeed = 0.125f;

    Vector3 offset;


    private void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        if (Input.GetKeyDown("v"))
        {
            offview = -offview;
            offset.x = offview;
        }

        Vector3 desiredPosition = target.position + offset.x * right - offset.z * forward;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
