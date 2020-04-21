﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControler : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeedX = 0.01f;
    private float rotationSpeedY = 0.1f;
    private float gravity = 3f;

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

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

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

        controller.Move(desiredMoveDirection * movementSpeed * Time.deltaTime);
        controller.Move(gravityVector * Time.deltaTime);
    }
}