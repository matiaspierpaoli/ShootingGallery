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

    //TODO: TP2 - Remove unused methods/variables/classes
    [SerializeField] private float groundDrag;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatisGrounded;
    //TODO: TP2 - Syntax - Consistency in naming convention
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
        SpeedControl();

        //TODO: Fix - Unnecessary code
        // get current orientation
        orientationForward = orientation.forward;
        orientationRight = orientation.right;
    }

    private void FixedUpdate()
    {
        moveDir = orientationForward * _movementDirection.y + orientationRight * _movementDirection.x;
        rigidBody.velocity = new Vector3(moveDir.x * speed, 0, moveDir.z * speed);
    }

    //TODO: Fix - Unclear name
    public void ProcessMove(Vector2 movementDirection)
    {
        _movementDirection = movementDirection;
    }

    //TODO: Fix - Unclear name
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
