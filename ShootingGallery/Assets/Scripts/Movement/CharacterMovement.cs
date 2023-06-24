using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float speed;

    private Vector3 moveDir;

    private Vector2 _movementDirection;

    private void OnEnable()
    {
        InputManager.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        InputManager.MoveEvent -= OnMove;
    }

    private void OnValidate()
    {
        rigidBody ??= GetComponent<Rigidbody>();
    }

    private void Update()
    {
        LimitSpeed();
    }

    private void FixedUpdate()
    {
        moveDir = orientation.forward * _movementDirection.y + orientation.right * _movementDirection.x;
        rigidBody.velocity = new Vector3(moveDir.x * speed, 0, moveDir.z * speed);
    }

    private void OnMove(Vector2 movementInput)
    {
        SetMovementDir(movementInput);
    }

    public void SetMovementDir(Vector2 movementDirection)
    {
        _movementDirection = movementDirection;
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z); 
        }
    }
}
