using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

namespace Movement
{

    public class MovementController : MonoBehaviour
    {
        public float movementSpeed = 7f;
        private float currentSpeed = 0f;
        private float speedSmoothVelocity = 0.5f;
        private readonly float speedSmoothTime = 0.1f;

        #region Rotation Speed

        private float rotationSpeedX = 0.01f;
        private float rotationSpeedY = 0.01f;

        #endregion

        private float _gravity = 3f;
        private Vector2 _movementInput;
        private float _ind = 1f;
        private Vector3 forward;
        private Vector3 right;

        #region Other Moves

        private bool isCrouched = false;
        public bool isRunning = false;
        private bool isVaulting = false;
        
        
        #endregion

        #region Vault

        private float _originOffset = 0.2f;
        private float _vaultMaxHeight = 0.6f;
        private float _lookDownDistance = 3f;
        private float _vaultableDistance = 2f;

        private float _vaultSpeed = 0.06f;
        
        private Vector3 _endPosition;
        private RaycastHit hit;

        #endregion

        private Transform mainCameraTransform;
        private CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            mainCameraTransform = Camera.main.transform;
        }

        #region Implementarea functiilor

        private void ToVault()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            StartCoroutine(Vault());
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

        #endregion

        private void Update()
        {
            GetInput();
            
            if (CanVault()) ToVault();

            if (Input.GetKeyDown(KeyCode.C)) ToCrouch();
            
            if (_movementInput != Vector2.zero)
            {
                ToMove();
                ToRun();
            }
            else if (isRunning) StopSprint();
            
            if (!isVaulting) GravityActionOnPlayer();
        }

        #region Gravity

        private void GravityActionOnPlayer()
        {
            var gravityVector = Vector3.zero;
            if (!controller.isGrounded) gravityVector.y -= _gravity;
            controller.Move(gravityVector * Time.deltaTime);
        }

        #endregion
        
        #region Input
        private void GetInput() => _movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        #endregion
        
        #region NormalMove
        private void NormalMove()
        {
            CameraDirections();

            PlayerRotate();
            
            Move();
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
            var desiredMoveDirectionX = (right * _movementInput.x).normalized;
            var desiredMoveDirectionY = (forward * _movementInput.y).normalized;
            
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

        private void Move()
        {
            var desiredMoveDirection = (right * _movementInput.x + forward * _movementInput.y).normalized;
            var targetSpeed = movementSpeed * _movementInput.magnitude;
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

        private bool CanVault()
        {
            var origin = transform.position;
            var direction = transform.forward;

            origin.y -= _originOffset;

            Debug.DrawRay(origin, direction * _vaultableDistance);
            if (!Physics.Raycast(origin, direction, out hit, _vaultableDistance)) return false;

            var origin2 = origin;
            origin2.y += _vaultMaxHeight;

            Debug.DrawRay(origin2, direction * _vaultableDistance);
            if (Physics.Raycast(origin2, direction, out hit, _vaultableDistance)) return false;
            
            var origin3 = origin2 + (direction * _vaultableDistance);
            
            Debug.DrawRay(origin3, -Vector3.up * _lookDownDistance);
            Physics.Raycast(origin3, -Vector3.up, out hit, _lookDownDistance);

            return true;
        }

        private IEnumerator Vault()
        {
            _endPosition = hit.point;
            _endPosition += (Vector3.up);
            
            isVaulting = true;

            while (transform.position != _endPosition)
            {
                var targetPosition = Vector3.Lerp(transform.position, _endPosition, _vaultSpeed);
                transform.position = targetPosition;
                yield return null;
            }

            isVaulting = false;
            yield break;
        }

        #endregion
    }
}