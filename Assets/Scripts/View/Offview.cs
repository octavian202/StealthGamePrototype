using UnityEngine;
using Cinemachine;

public class Offview : MonoBehaviour
{
    public Transform playerBody;
    public Transform objLookingAt;
    public float ofX;
    public float moveSpeed;
    public float height;
    public float front;

    public CinemachineFreeLook freeLookCam;

   

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            ofX = -ofX;
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

        //freeLookCam.m_XAxis.Value = 

    }
}
