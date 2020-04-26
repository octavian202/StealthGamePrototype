using UnityEngine;

public class MovementControler : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0.5f;
    private float speedSmoothTime = 0.1f;

    #region rotation speed
    [Header("Rotation speed")]
    [SerializeField] private float rotationSpeedX = 0.01f;
    [SerializeField] private float rotationSpeedY = 0.01f;
    #endregion

    private float gravity = 3f;

    #region Other moves
    private bool isCrouched = false;
    private float ind = 1f;
    public bool isRunning = false;
    #endregion

    private Transform mainCameraTransform;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.C)) Crouch();

        if (Input.GetKey(KeyCode.LeftShift)) Sprint();
        else stopSprint();

        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (right * movementInput.x + forward * movementInput.y).normalized;
        Vector3 desiredMoveDirectionX = (right * movementInput.x).normalized;
        Vector3 desiredMoveDirectionY = (forward * movementInput.y).normalized;
        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded) gravityVector.y -= gravity;

        if (desiredMoveDirectionX != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirectionX), rotationSpeedX);
        }
        if (desiredMoveDirectionY != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirectionY), rotationSpeedY);
        }

        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * ind * Time.deltaTime);
        controller.Move(gravityVector * Time.deltaTime);
    }

    void Crouch()
    {
        isCrouched = !isCrouched;

        if (isRunning && isCrouched) isRunning = false;


        if (isCrouched) ind = 0.5f;
        else ind = 1f;

        transform.localScale = new Vector3(1f, ind, 1f);
    }

    #region Sprint
    void Sprint()
    {
        isRunning = true;
        ind = 2f;
    }

    void stopSprint()
    {
        isRunning = false;
        ind = 1f;
    }

    #endregion
}
