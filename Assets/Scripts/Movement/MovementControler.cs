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

    #region other moves
    private bool isCrouched = false;
    private float ind = 1f;
    public bool isRunning = false;
    #endregion

    private Transform mainCameraTransform;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        mainCameraTransform = Camera.main.transform; //camera
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.C)) Crouch();  //crouch

        if (Input.GetKey(KeyCode.LeftShift)) Sprint();
        else stopSprint();
        //fuge sau nu fuge

        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));  //input

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;
        //fatza si dreapta camerei

        forward.y = 0f;
        right.y = 0f;
        //sa nu mearga in sus

        forward.Normalize();
        right.Normalize();
        //sa nu mearga prea mult

        Vector3 desiredMoveDirection = (right * movementInput.x + forward * movementInput.y).normalized; //directia in care trebuie sa mearga
        Vector3 desiredMoveDirectionX = (right * movementInput.x).normalized; //desiredMoveDirection x
        Vector3 desiredMoveDirectionY = (forward * movementInput.y).normalized; //desiredMoveDirection y
        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded) gravityVector.y -= gravity;  // daca e in aer cade

        if (desiredMoveDirectionX != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirectionX), rotationSpeedX);  //se roteste in dreapta sau in stanga
        }
        if (desiredMoveDirectionY != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirectionY), rotationSpeedY);  // se roteste sa mearga in fatza sau in spate
        }

        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * ind * Time.deltaTime);  // merge in directia in care trebuie
        controller.Move(gravityVector * Time.deltaTime);  // gravitatie
    }

    void Crouch()
    {
        isCrouched = !isCrouched; // toggle

        if (isRunning && isCrouched) stopSprint();  // sa nu fie si crouch si sa si fuga deodata

        if (isCrouched) ind = 0.5f; // sa incetineasca daca e crouch
        else ind = 1f;  // sa mearga normal daca nu e crouch

        transform.localScale = new Vector3(1f, ind, 1f);  // se face marimea potrivita
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
