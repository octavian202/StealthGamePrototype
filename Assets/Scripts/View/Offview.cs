using UnityEngine;
using Cinemachine;

public class Offview : MonoBehaviour
{
    public Transform playerBody;
    public CinemachineFreeLook freeLookCam;

    private float ofX = 2.5f;
    private float moveSpeed = 0.125f;
    private float height = 0.1f;
    private float front = 2f;
    private float camFix = -35f;
    private Vector3 endPos;
    private float sprintHeight = 1.5f;

    private bool sprint = false;

    private MovementControler movementcontroler;

    private void Start()
    {
        movementcontroler = GameObject.Find("Player").GetComponent<MovementControler>();
    }

    private void Update()
    {
        sprint = movementcontroler.isRunning;

        Vector3 up = new Vector3(0f, 1f, 0f);

        if (sprint)
        {
            endPos = playerBody.position + sprintHeight * up;
        } else
        {
            if (Input.GetKeyDown("v"))
            {
                camFix = -camFix;
                ofX = -ofX;
                freeLookCam.m_XAxis.Value += camFix;
            }

            Vector3 forward = playerBody.forward;
            Vector3 right = playerBody.right;

            forward.y = 0;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            endPos = playerBody.position + ofX * right + height * up + front * forward;
        }

        transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed);

    }
}
