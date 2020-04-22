using UnityEngine;
using Cinemachine;

public class Offview : MonoBehaviour
{
    public Transform playerBody;
    public CinemachineFreeLook freeLookCam;

    private float ofX = 2.5f;
    private float moveSpeed = 0.3f;
    private float height = 0.1f;
    private float front = 2f;
    private float camFix = -35f;

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            camFix = -camFix;
            ofX = -ofX;
            freeLookCam.m_XAxis.Value += camFix;
        }


        Vector3 up = new Vector3(0f, 1f, 0f);

        Vector3 forward = playerBody.forward;
        Vector3 right = playerBody.right;

        forward.y = 0;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 endPos = playerBody.position + ofX * right + height * up + front * forward;
        transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed);

        

    }
}
