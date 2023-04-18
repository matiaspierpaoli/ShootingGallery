using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private const int MaxFloorDistance = 10;
    
    [Header ("SetUp")]
    [SerializeField] private Rigidbody rigidBody;
    
    [SerializeField] private Transform feetPivot;

    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float speed;

    [SerializeField] private float minJumpDistance;
    private Vector3 _currentMovement;

    [Range(0, 1000)]
    [SerializeField] private float jumpForce = 10;

    //[SerializeField] private float jumpBufferTime;
    private Coroutine _jumpCoruotine;
    [SerializeField] private float bufferTime;


    //private bool _isJumpInput;
    [SerializeField] private float coyoteTime;


    private void OnValidate()
    {
        rigidBody ??= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = _currentMovement* speed + Vector3.up* rigidBody.velocity.y;
    }

    public void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();
        _currentMovement = new Vector3(movement.x, _currentMovement.y, movement.y);
        //_currentMovement = (movement.x * orientation.right + movement.y * orientation.forward);
    }
     
    public void OnJump()
    {
        if (_jumpCoruotine != null) 
            StopCoroutine(JumpCoroutine(bufferTime));

        _jumpCoruotine = StartCoroutine(JumpCoroutine(bufferTime));

    }


    private void OnSprint(InputValue input)
    {
        Debug.Log($"sprint: {input.isPressed}");
         
    }

    //private void CancelJumpInput()
    //{
    //    _isJumpInput = false;
    //}

    private IEnumerator JumpCoroutine(float bufferTime)
    {

        if (!feetPivot)
            yield break;

        //while (timeElapsed <= bufferTime)
        //{           
        for (var timeElapsed = 0.0f; timeElapsed <= bufferTime; timeElapsed += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();

            var isFalling = rigidBody.velocity.y <= 0;
            var currentFeetPosition = feetPivot.position;

            var canNormalJump = isFalling && CanJumpInPosition(currentFeetPosition);

            var coyoteTimeFeetPosition = currentFeetPosition - rigidBody.velocity * coyoteTime;
            var canCoyoteJump = isFalling && CanJumpInPosition(coyoteTimeFeetPosition);

            if (!canNormalJump && canCoyoteJump)
            {
                Debug.DrawLine(currentFeetPosition, coyoteTimeFeetPosition, Color.cyan, 5f);
            }

            if (canNormalJump || canCoyoteJump)
            {
                var jumpForceVector = Vector3.up * jumpForce;

                //Esto cancela la velocidad de caida.
                if (rigidBody.velocity.y < 0)
                    jumpForceVector.y -= rigidBody.velocity.y;

                rigidBody.AddForce(jumpForceVector, ForceMode.Impulse);

                if (timeElapsed > 0)
                    Debug.Log($"<color=grey>{name}: buffered jump for {timeElapsed} seconds</color>");

                yield break;
            }


        }    
    }

    private bool CanJumpInPosition(Vector3 currentFeetPosition)
    {
        //La variable hit puede ser presentada dirertamente en la llamada al metodo Raycast
        //La keyword out significa que le damos acceso al metodo para asignarle un valor a nuestra variable.
        //Ojo! El valor con el que termina podria ser nulo en otros metodos, pero en el caso del raycast, nunca sera asi
        return Physics.Raycast(currentFeetPosition, Vector3.down, out var hit, MaxFloorDistance)
               && hit.distance <= minJumpDistance;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(feetPivot.position, feetPivot.position + Vector3.down * minJumpDistance);
    //}
}
