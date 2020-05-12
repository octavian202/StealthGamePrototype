using UnityEngine;

namespace Movement
{

    public class MovementController : MonoBehaviour
    {
        public float movementSpeed = 7f;
        private float currentSpeed = 0f;
        private float speedSmoothVelocity = 0.5f;
        private float speedSmoothTime = 0.1f;

        #region rotation speed

        private float rotationSpeedX = 0.01f;
        private float rotationSpeedY = 0.01f;

        #endregion

        private float _gravity = 3f;
        private Vector2 _movementInput;
        private float _ind = 1f;
        private Vector3 forward;
        private Vector3 right;

        #region Other moves

        private bool isCrouched = false;
        public bool isRunning = false;
        private bool isVaulting = false;
        
        
        #endregion

        #region Vault

        public float _originOffset = 0.1f;
        public float _vaultableDistance = 10f;
        public float _maxVaultHeight;
        public RaycastHit hit;
        
        #endregion

        private Transform mainCameraTransform;
        private CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            mainCameraTransform = Camera.main.transform;
        }

        private void ToMove()
        {
            if (isVaulting) return;
            NormalMove();
        }

        private void ToCrouch()
        {
            if (isRunning) return;
            Crouch();
        }

        private void ToRun()
        {
            if (Input.GetKey(KeyCode.LeftShift)) StartSprint();
            if (Input.GetKeyUp(KeyCode.LeftShift)) StopSprint();
        }

        private void ToVault()
        {
            if (!CanVault()) return;
        }

        private void Update()
        {
            GetInput();
            
            ToVault();

            if (Input.GetKeyDown(KeyCode.C)) ToCrouch();
            
            if (_movementInput != Vector2.zero)
            {
                ToMove();
                ToRun();
            }
            else if (isRunning) StopSprint();
        }

        #region Input
        private Vector2 GetInput() => _movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        #endregion
        
        #region NormalMove
        private void NormalMove()
        {
            CameraDirections();

            PlayerRotate();
            
            Move();

            GravityActionOnPlayer();
        }
        
        private void CameraDirections()
        {
            forward = mainCameraTransform.forward;
            right = mainCameraTransform.right;
            
            forward.y = 0f;
            right.y = 0f;
            
            forward.Normalize();
            right.Normalize();
        }

        private void PlayerRotate()
        {
            Vector3 desiredMoveDirectionX = (right * _movementInput.x).normalized;
            Vector3 desiredMoveDirectionY = (forward * _movementInput.y).normalized;
            
            if (desiredMoveDirectionX != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(desiredMoveDirectionX), rotationSpeedX);
            }

            if (desiredMoveDirectionY != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(desiredMoveDirectionY), rotationSpeedY);
            }
        }

        private void GravityActionOnPlayer()
        {
            Vector3 gravityVector = Vector3.zero;
            if (!controller.isGrounded) gravityVector.y -= _gravity;
            controller.Move(gravityVector * Time.deltaTime);
        }

        private void Move()
        {
            Vector3 desiredMoveDirection = (right * _movementInput.x + forward * _movementInput.y).normalized;
            float targetSpeed = movementSpeed * _movementInput.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

            controller.Move(currentSpeed * _ind * Time.deltaTime * desiredMoveDirection);
        }
        #endregion

        #region Crouch

        private void Crouch()
        {
            isCrouched = !isCrouched;

            _ind = 1f;
            if (isCrouched) _ind = 0.5f;

            transform.localScale = new Vector3(1f, _ind, 1f);
        }

        #endregion

        #region Sprint

        private void StartSprint()
        {
            if (isCrouched) Crouch();

            _ind = 1.5f;
            isRunning = true;
        }

        private void StopSprint()
        {
            _ind = 1f;
            isRunning = false;
        }

        #endregion

        #region Vault

        public bool CanVault()
        {
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;
            
            origin.y -= _originOffset;

            Debug.DrawRay(origin, direction * _vaultableDistance, Color.black);
            if (!Physics.Raycast(origin, direction, out hit, _vaultableDistance)) return false;

            Vector3 origin2 = origin;
            origin2.y += _maxVaultHeight;

            Debug.DrawRay(origin2, direction * _vaultableDistance, Color.black);
            if (Physics.Raycast(origin2, direction, out hit, _vaultableDistance)) return false;

            return true;
        }

        #endregion
        
    }
}