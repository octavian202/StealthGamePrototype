using UnityEngine;
using Cinemachine;

public class Offview : MonoBehaviour
{
    public Transform playerBody;
    public CinemachineFreeLook freeLookCam;

    #region walking variables
    [SerializeField] private float ofX = 2.5f;
    [SerializeField] private float moveSpeed = 0.125f;
    [SerializeField] private float height = 0.1f;
    [SerializeField] private float front = 2f;
    #endregion

    private float camFix = -35f;
    private Vector3 endPos;

    [SerializeField] private float sprintHeight = 1.5f;

    private bool isRunning;
    private MovementControler movementcontroler;

    private void Start()
    {
        movementcontroler = GameObject.Find("Player").GetComponent<MovementControler>();
    }

    private void Update()
    {
        isRunning = movementcontroler.isRunning; //verifica daca fuge

        Vector3 up = new Vector3(0f, 1f, 0f); // folosit pt ridicarea camerei

        if (isRunning) endPos = playerBody.position + sprintHeight * up; // muta camera pe player cand fuge
        else
        {
            if (Input.GetKeyDown("v")) // daca apasa V se schimba partea de pe care vede pe mr bean
            {
                camFix = -camFix;
                ofX = -ofX;
                freeLookCam.m_XAxis.Value += camFix;
            }

            Vector3 forward = playerBody.forward;
            Vector3 right = playerBody.right;
            //directiile

            forward.y = 0f;
            right.y = 0f;
            // nu in sus

            forward.Normalize();
            right.Normalize();
            // il face vector cu magnitudinea 1

            endPos = playerBody.position + ofX * right + height * up + front * forward; // pozitia de langa mr bean
        }

        transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed); // se duce pe poz langa mr bean

    }
}
