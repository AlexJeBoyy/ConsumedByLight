using Cinemachine;
using System;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] Transform CameraFollow;
    [SerializeField] PlayerInput _input;
    [SerializeField] CameraController _cameraController;
    Rigidbody _rigidbody = null;
    CapsuleCollider _capsuleCollider = null;


    Vector3 _playerMoveInput;

    Vector3 _playerLookInput;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    [SerializeField] float _cameraPitch = 0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    [Header("Movement")]
    [SerializeField] private float _currentSpeed; //Shown for debugging
    [SerializeField] float _movementMultiplier = 40f;
    [SerializeField] float _notGroundedMultiplier = 1.25f;
    [SerializeField] float _rotationSpeedMultiplier = 500;
    [SerializeField] float _pitchSpeedMultiplier = 500;
    [SerializeField] float _runMultiplier = 40f;

    [Header("Ground Check")]
    public bool _playerIsGrounded = true;
    [SerializeField][Range(0f, 1.8f)] float _groundCheckRaduisMultiplier = .9f;
    [SerializeField][Range(-.95f, 1.05f)] float _groundCheckDistance = .05f;
    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float _gravityFallCurrent = -5f;
    [SerializeField] float _gravityFallMin = -5f;
    [SerializeField] float _gravityFallMax = -75f;
    [SerializeField][Range(-5f, -35f)] float _gravityFallIncrementAmount = -20f;
    [SerializeField] float _gravityFallIncrementTime = -.05f;
    [SerializeField] float _playerFallTimer = -0f;
    [SerializeField] float _gravityGrounded = -1f;
    [SerializeField] float _maxSlopeAngle = 47.5f;

    [Header("Jumping")]
    [SerializeField] float _initialJumpForce = 1000f;
    [SerializeField] float _continualJumpForceMultiplier = .1f;
    [SerializeField] float _jumpTime = .175f;
    [SerializeField] float _jumpTimeCounter = 0f;
    [SerializeField] float _coyoteTime = 0.15f;
    [SerializeField] float _coyoteTimeCounter = 0f;
    [SerializeField] float _jumpBufferTime = .2f;
    [SerializeField] float _jumpBufferTimeCounter = 0f;
    [SerializeField] bool _playerIsJumping = false;
    [SerializeField] bool _jumpWasPressedLastFrame = false;

    [Header("Slide-Dash")]
    [SerializeField] private bool _isSlideDashing = false;
    [SerializeField] private float _slideDashInitialSpeed = 20f;
    [SerializeField] private float _slideDashDeceleration = 5f;
    [SerializeField] private float _slideDashDuration = 1.5f;
    [SerializeField] private float _slideDashTimer = 0f;
    [SerializeField] private float _slideDashColliderHeight = 1f;

    private float _originalColliderHeight;
    private Vector3 _slideDashDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _originalColliderHeight = _capsuleCollider.height;
    }

    private void FixedUpdate()
    {
        _playerLookInput = GetLookInput();
        PlayerLook();

        PlayerFOV();

        PitchCamera();

        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerIsGroundedCheck();

        _playerMoveInput = PlayerMove();
        _playerMoveInput = PlayerSlope();
        _playerMoveInput = PlayerRun();


        if (_input.DashIsPressed && _playerIsGrounded && !_isSlideDashing && _input.MoveIsPressed)
        {
            StartSlideDash();
        }

        if (_isSlideDashing)
        {
            SlideDash();
        }

        _playerMoveInput.y = PlayerFallGravity();
        _playerMoveInput.y = PlayerJump();

        _playerMoveInput *= _rigidbody.mass; // Note: Dev purposes

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);

        // Showing speed
        if (!_isSlideDashing)
        {
            _currentSpeed = _rigidbody.velocity.magnitude;

        }
        // Debug.Log("Current Speed: " + _currentSpeed.ToString("F2") + " units/sec");
        //Debug.Log(_playerLookInput);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);

    }
    private void PlayerLook()
    {
        ////Node: the commented out stuff maes it so that if the player is slide dashing you can rotate the came without rotating the player 
        //if (!_isSlideDashing)
        //{
        _rigidbody.rotation = Quaternion.Euler(0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0f);
        //}
        //else
        //{
        //    float yaw = _playerLookInput.x * _rotationSpeedMultiplier;
        //    CameraFollow.rotation = Quaternion.Euler(CameraFollow.rotation.eulerAngles.x, CameraFollow.rotation.eulerAngles.y + yaw, CameraFollow.rotation.eulerAngles.z);
        //}

    }
    private void PitchCamera()
    {
        Vector3 rotationValue = CameraFollow.rotation.eulerAngles;
        _cameraPitch += _playerLookInput.y * _pitchSpeedMultiplier;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.9f, 89.9f);
        CameraFollow.rotation = Quaternion.Euler(_cameraPitch, rotationValue.y, rotationValue.z);
    }
    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private bool PlayerIsGroundedCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRaduisMultiplier;
        float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y - sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_rigidbody.position, sphereCastRadius, Vector3.down, out _groundCheckHit, sphereCastTravelDistance);
    }
    private Vector3 PlayerMove()
    {
        return ((_playerIsGrounded) ? (_playerMoveInput * _movementMultiplier) : (_playerMoveInput * _movementMultiplier * _notGroundedMultiplier));
    }
    private Vector3 PlayerSlope()
    {
        Vector3 calculatedPlayerMovement = _playerMoveInput;


        if (_playerIsGrounded)
        {
            Vector3 localGroundCheckHitNormal = _rigidbody.transform.InverseTransformDirection(_groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rigidbody.transform.up);

            if (groundSlopeAngle == 0.0f)
            {
                if (_input.MoveIsPressed)
                {
                    RaycastHit rayHit;
                    float rayHeightFromGround = .1f;
                    float rayCalculatedRayHeight = _rigidbody.position.y - _capsuleCollider.bounds.extents.y + rayHeightFromGround;
                    Vector3 rayOrgin = new Vector3(_rigidbody.position.x, rayCalculatedRayHeight, _rigidbody.position.z);
                    if (Physics.Raycast(rayOrgin, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), out rayHit, .75f))
                    {
                        if (Vector3.Angle(rayHit.normal, _rigidbody.transform.up) > _maxSlopeAngle)
                        {
                            calculatedPlayerMovement.y = -_movementMultiplier;
                        }
                    }
                    Debug.DrawRay(rayOrgin, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), Color.green, 1f);
                }

                if (calculatedPlayerMovement.y == 0f)
                {
                    calculatedPlayerMovement.y = _gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rigidbody.transform.up) - 90.0f;
                calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / 90f);

                if (groundSlopeAngle < _maxSlopeAngle)
                {
                    if (_input.MoveIsPressed)
                    {
                        calculatedPlayerMovement.y += _gravityGrounded;
                    }
                }
                else
                {
                    float calculateSlopeGravity = groundSlopeAngle * -.2f;
                    if (calculateSlopeGravity < calculatedPlayerMovement.y)
                    {
                        calculatedPlayerMovement.y = calculateSlopeGravity;
                    }

                }
            }
#if UNITY_EDITOR
            Debug.DrawRay(_rigidbody.position, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), Color.red, 0.5f);
#endif
        }
        return calculatedPlayerMovement;
    }
    private Vector3 PlayerRun()
    {
        Vector3 calculatePlayerRunSpeed = _playerMoveInput;

        if (_input.RunIsPressed && _input.MoveIsPressed)
        {
            float runSpeed = Mathf.Lerp(_movementMultiplier, _runMultiplier, Time.deltaTime * 5f); // Smooth transition
            calculatePlayerRunSpeed *= runSpeed;
        }
        else
        {
            calculatePlayerRunSpeed *= _movementMultiplier;
        }

        return calculatePlayerRunSpeed;
    }
    private void PlayerFOV()
    {
        CinemachineVirtualCamera playerCam = _cameraController.c1Person;
        float endFOV;
        float transitionTime = 7f;
        float minFOV = 60;
        float maxFOV = 90;
        if (_input.RunIsPressed && _input.MoveIsPressed || _isSlideDashing)
        {
            endFOV = 90;
            _cameraController.ChangeFOV(playerCam, endFOV, transitionTime, minFOV, maxFOV);
        }
        else
        {
            endFOV = 60;
            _cameraController.ChangeFOV(playerCam, endFOV, transitionTime, minFOV, maxFOV);
        }

    }

    private float PlayerFallGravity()
    {
        float gravity = _playerMoveInput.y;
        if (_playerIsGrounded)
        {
            _gravityFallCurrent = _gravityFallMin; // Reset
        }
        else
        {
            _playerFallTimer -= Time.fixedDeltaTime;
            if (_playerFallTimer < 0.0f)
            {
                if (_gravityFallCurrent > _gravityFallMax)
                {
                    _gravityFallCurrent += _gravityFallIncrementAmount;
                }
                _playerFallTimer = _gravityFallIncrementTime;

            }
            gravity = _gravityFallCurrent;
        }
        return gravity;
    }

    private float PlayerJump()
    {
        float calculateJumpInput = _playerMoveInput.y;

        SetJumpTimeCounter();
        SetCoyoteTimeCounter();
        SetJumpBufferTimeCounter();

        if (_jumpBufferTimeCounter > 0f && !_playerIsJumping && _coyoteTimeCounter > 0f)
        {
            ////Cant jump when you're on a slope greater than the slope angle
            //if (Vector3.Angle(_rigidbody.transform.up, _groundCheckHit.normal) < _maxSlopeAngle) 
            //{
            calculateJumpInput = _initialJumpForce;
            _playerIsJumping = true;
            _jumpBufferTimeCounter = 0f;
            _coyoteTimeCounter = 0f;
            //}

            if (_isSlideDashing)
            {
                EndSlideDash();
            }
        }
        else if (_input.JumpIsPressed && _playerIsJumping && !_playerIsGrounded && _jumpTimeCounter > 0f)
        {
            calculateJumpInput = _initialJumpForce * _continualJumpForceMultiplier;
        }
        else if (_playerIsGrounded && _playerIsJumping)
        {
            _playerIsJumping = false;
        }
        return calculateJumpInput;
    }

    private void SetJumpBufferTimeCounter()
    {
        if (_playerIsJumping)
        {
            _jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            _jumpTimeCounter = _jumpTime;
        }
    }

    private void SetCoyoteTimeCounter()
    {
        if (_playerIsGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void SetJumpTimeCounter()
    {
        if (!_jumpWasPressedLastFrame && _input.JumpIsPressed)
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }
        else if (_jumpBufferTimeCounter > 0f)
        {
            _jumpBufferTimeCounter -= Time.fixedDeltaTime;
        }
        _jumpWasPressedLastFrame = _input.JumpIsPressed;
    }

    private void StartSlideDash()
    {


        _slideDashDirection = _rigidbody.transform.forward;
        _slideDashInitialSpeed = _currentSpeed;

        if (_slideDashDirection == Vector3.zero)
        {
            Vector3 moveInputDirection = new Vector3(_input.MoveInput.x, 0f, _input.MoveInput.y).normalized;
            _slideDashDirection = _rigidbody.transform.TransformDirection(moveInputDirection);
        }

        _isSlideDashing = true;
        _slideDashTimer = _slideDashDuration;


        _capsuleCollider.height = _slideDashColliderHeight;
        _capsuleCollider.center = new Vector3(0f, _slideDashColliderHeight / 2f, 0f);

        _rigidbody.AddForce(_slideDashDirection * _slideDashInitialSpeed, ForceMode.Impulse);
    }

    private void SlideDash()
    {

        if (_slideDashTimer > 0f)
        {
            _slideDashTimer -= Time.fixedDeltaTime;

            _rigidbody.AddForce(_slideDashDirection * _slideDashDeceleration * Time.fixedDeltaTime, ForceMode.Acceleration);

            float speedReduction = Mathf.Lerp(_slideDashInitialSpeed, 0f, 1 - (_slideDashTimer / _slideDashDuration));

            _rigidbody.AddForce(_slideDashDirection * speedReduction * Time.fixedDeltaTime, ForceMode.Acceleration);


            if (_rigidbody.velocity.magnitude < 0.1f)
            {
                EndSlideDash();
            }
        }
        else
        {
            EndSlideDash();
        }
    }

    private void EndSlideDash()
    {

        //ResetRotation();

        _capsuleCollider.height = _originalColliderHeight;
        _capsuleCollider.center = new Vector3(0f, 0f, 0f);
        _isSlideDashing = false;
    }

    //Note: the reset rotation function does not work because if you rotate the player the camera rotates with the player instead of staying loose from it and not rotating with it
    private void ResetRotation()
    {
        float camTargetRotationX = _cameraController.MainCam.transform.rotation.y;
        float camTargetRotationY = _cameraController.MainCam.transform.rotation.y;
        float camTargetRotationZ = _cameraController.MainCam.transform.rotation.y;

        float myTargetRotationX = 0f;
        float myTargetRotationY = _cameraController.MainCam.transform.rotation.y;
        float myTargetRotationZ = 0f;
        Vector3 myEulerAngleRotation = new Vector3(myTargetRotationX, myTargetRotationY, myTargetRotationZ);
        _rigidbody.transform.rotation = Quaternion.Euler(myEulerAngleRotation);

        Vector3 camEulerAngleRotation = new Vector3(camTargetRotationX, camTargetRotationY, camTargetRotationZ);
        _cameraController.MainCam.transform.rotation = Quaternion.Euler(camEulerAngleRotation);
        Debug.Log("reset rotation" + camEulerAngleRotation);

    }


}
