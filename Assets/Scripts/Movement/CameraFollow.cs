using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offview = 2f;
    public Transform mainCamera;

    public float smoothSpeed = 0.125f;

    Vector3 offset = new Vector3(0f, 0.5f, 0f);

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            offview = -offview;
            offset.x = offview;
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = target.forward;
        Vector3 right = target.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredPosition = target.position + offset.x * right - offset.z * forward + offset.y * new Vector3(1f, 1f, 1f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
