using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float speed;

    [SerializeField] private float groundDrag;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatisGrounded;
    private bool grounded;

    private Vector3 orientationForward;  
    private Vector3 orientationRight;  
    private Vector3 moveDir;

    private Vector2 _movementDirection;

    private void OnValidate()
    {
        rigidBody ??= GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatisGrounded);

        SpeedControl();

        //if (grounded)
        //    rigidBody.drag = groundDrag;
        //else
        //    rigidBody.drag = 0;


        // get current orientation
        orientationForward = orientation.forward;
        orientationRight = orientation.right;
    }

    private void FixedUpdate()
    {
        moveDir = orientationForward * _movementDirection.y + orientationRight * _movementDirection.x;
        rigidBody.velocity = new Vector3(moveDir.x * speed, 0, moveDir.z * speed);
        //rigidBody.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);
    }

    public void ProcessMove(Vector2 movementDirection)
    {
        _movementDirection = movementDirection;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z); 
        }
    }
}
