using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform orientation;
    [SerializeField] private float gravity;
    [SerializeField] GameData gameData;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float flashSpeed;
    [SerializeField] private bool isflashSpeedEnabled;

    [NonSerialized] public bool canMove = false;

    private Vector3 moveDir;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private float originalSpeed;

    private Vector2 _movementDirection;

    private void OnEnable()
    {
        InputManager.MoveEvent += OnMove;
        InputManager.FlashEvent += OnFlashCheat;

        if (gameData.isNextLevelCheatAvailiable)
            GameManager.ReplayEvent += ResetTransform;
    }

    private void OnDisable()
    {
        InputManager.MoveEvent -= OnMove;
        InputManager.FlashEvent -= OnFlashCheat;

        if (gameData.isNextLevelCheatAvailiable)
            GameManager.ReplayEvent -= ResetTransform;
    }

    private void OnValidate()
    {
        rigidBody ??= GetComponent<Rigidbody>();
    }

    private void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        originalSpeed = speed;
    }

    private void Update()
    {
        if (!canMove) return;

        LimitSpeed();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        moveDir = orientation.forward * _movementDirection.y + orientation.right * _movementDirection.x;

        Vector3 gravityVector = Vector3.down * gravity;

        Vector3 velocity = new Vector3(moveDir.x * speed, rigidBody.velocity.y, moveDir.z * speed);

        rigidBody.velocity = velocity + gravityVector;
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

    public void ResetTransform()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;
    }

    public void OnFlashCheat()
    {
        isflashSpeedEnabled = !isflashSpeedEnabled;

        if (isflashSpeedEnabled)
        {
            speed = flashSpeed;
            Debug.Log("Flash Mode Enabled");
        }
        else
        {
            speed = originalSpeed;
            Debug.Log("Flash Mode Disabled");
        }
    }
}
