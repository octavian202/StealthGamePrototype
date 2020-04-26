using UnityEngine;
using Cinemachine;

public class Offview : MonoBehaviour
{
    public Transform playerBody;
    public CinemachineFreeLook freeLookCam;

    #region Walking variables
    [SerializeField] private float ofX = 2.5f;
    [SerializeField] private float moveSpeed = 0.125f;
    [SerializeField] private float height = 0.1f;
    [SerializeField] private float front = 2f;
    #endregion

    private float camFix = -35f;
    private Vector3 endPos;

    [SerializeField] private float sprintHeight = 1.5f;

    private bool isRunning = false;
    private MovementControler movementcontroler;

    private void Start()
    {
        movementcontroler = GameObject.Find("Player").GetComponent<MovementControler>();
    }

    private void Update()
    {
        isRunning = movementcontroler.isRunning;

        Vector3 up = new Vector3(0f, 1f, 0f);

        if (isRunning) endPos = playerBody.position + sprintHeight * up;
        else
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
