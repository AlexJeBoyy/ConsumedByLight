using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform CameraFollow;
    [SerializeField] PlayerInput _input;
    [SerializeField] CameraController _cameraController;
    Rigidbody _rb = null;
    CapsuleCollider _capsuleCollider = null;


    Vector3 _playerMoveInput;

    Vector3 _playerLookInput;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    [SerializeField] float _cameraPitch = 0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;
    [SerializeField] AudioSource _audioSource = null;
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    [Header("Movement")]
    [SerializeField] private float _currentSpeed; //Shown for debugging
    [SerializeField] float _movementMultiplier = 150;
    [SerializeField] float _notGroundedMultiplier = 1.25f;
    [SerializeField] float _rotationSpeedMultiplier = 500;
    [SerializeField] float _pitchSpeedMultiplier = 500;
    [SerializeField] float _runMultiplier = 40;

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
    private Vector3 _slideDashDirection;
    private float _originalColliderHeight;

    [Header("Dash")]
    [SerializeField] private bool _isDashing = false;
    [SerializeField] private bool _canDash = true;
    [SerializeField] private float _dashSpeed = 300f;
    [SerializeField] private float _startDashTimer = 4f;
    [SerializeField] private float _dashTime = .5f;
    [SerializeField] private float _dashCooldown = 4;
    [SerializeField] private AudioClip dashSound;

    private Vector3 _dashDirection;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _originalColliderHeight = _capsuleCollider.height;
    }

    private void Update()
    {
        if (!_canDash)
        {
            _dashCooldown -= Time.deltaTime;

            if (_dashCooldown <= 0.0f)
            {
                _canDash = true;
                _dashCooldown = _startDashTimer;
            }
        }
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
        //_playerMoveInput = PlayerRun();  //Note: deactivated running


        if (_input.SlideIsPressed && _playerIsGrounded && !_isSlideDashing && _input.MoveIsPressed && !_isDashing)
        {
            StartSlideDash();
        }

        if (_isSlideDashing && !_isDashing)
        {
            SlideDash();
        }

        if (_input.DashIsPressed && _canDash && !_isSlideDashing)
        {
            PlayerDash();
        }

        _playerMoveInput.y = PlayerFallGravity();
        _playerMoveInput.y = PlayerJump();

        _playerMoveInput *= _rb.mass; // Note: Dev purposes

        _rb.AddRelativeForce(_playerMoveInput, ForceMode.Force);

        // Showing speed
        if (!_isSlideDashing)
        {
            _currentSpeed = _rb.velocity.magnitude;

        }
        // Debug.Log("Current Speed: " + _currentSpeed.ToString("F2") + " units/sec");
        //Debug.Log(_playerLookInput);
    }
    #region Camera
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
        _rb.rotation = Quaternion.Euler(0f, _rb.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0f);
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

    private void PlayerFOV()
    {
        CinemachineVirtualCamera playerCam = _cameraController.c1Person;
        float endFOV;
        float transitionTime = 8f;
        float minFOV = 60;
        float maxFOV = 100;
        if (_input.MoveIsPressed)
        {
            endFOV = 75;
            _cameraController.ChangeFOV(playerCam, endFOV, transitionTime, minFOV, maxFOV);
        }
        else if (_isSlideDashing || _isDashing)
        {
            endFOV = 100;
            _cameraController.ChangeFOV(playerCam, endFOV, transitionTime, minFOV, maxFOV);
        }
        else
        {
            endFOV = 60;
            _cameraController.ChangeFOV(playerCam, endFOV, transitionTime, minFOV, maxFOV);
        }

    }
    #endregion

    #region Base Movement
    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private bool PlayerIsGroundedCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRaduisMultiplier;
        float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y - sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_rb.position, sphereCastRadius, Vector3.down, out _groundCheckHit, sphereCastTravelDistance);
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
            Vector3 localGroundCheckHitNormal = _rb.transform.InverseTransformDirection(_groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rb.transform.up);

            if (groundSlopeAngle == 0.0f)
            {
                if (_input.MoveIsPressed)
                {
                    RaycastHit rayHit;
                    float rayHeightFromGround = .1f;
                    float rayCalculatedRayHeight = _rb.position.y - _capsuleCollider.bounds.extents.y + rayHeightFromGround;
                    Vector3 rayOrgin = new Vector3(_rb.position.x, rayCalculatedRayHeight, _rb.position.z);
                    if (Physics.Raycast(rayOrgin, _rb.transform.TransformDirection(calculatedPlayerMovement), out rayHit, .75f))
                    {
                        if (Vector3.Angle(rayHit.normal, _rb.transform.up) > _maxSlopeAngle)
                        {
                            calculatedPlayerMovement.y = -_movementMultiplier;
                        }
                    }
                    Debug.DrawRay(rayOrgin, _rb.transform.TransformDirection(calculatedPlayerMovement), Color.green, 1f);
                }

                if (calculatedPlayerMovement.y == 0f)
                {
                    calculatedPlayerMovement.y = _gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rb.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rb.transform.up) - 90.0f;
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
            Debug.DrawRay(_rb.position, _rb.transform.TransformDirection(calculatedPlayerMovement), Color.red, 0.5f);
#endif
        }
        return calculatedPlayerMovement;
    }
    #endregion

    #region Running (deactivated)
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
    #endregion

    #region Gravity
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

    #endregion

    #region Jumping
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

    #endregion

    #region Sliding
    private void StartSlideDash()
    {
        _slideDashDirection = _rb.transform.forward;
        _slideDashInitialSpeed = _currentSpeed;

        if (_slideDashDirection == Vector3.zero)
        {
            Vector3 moveInputDirection = new Vector3(_input.MoveInput.x, 0f, _input.MoveInput.y).normalized;
            _slideDashDirection = _rb.transform.TransformDirection(moveInputDirection);
        }

        _isSlideDashing = true;
        _slideDashTimer = _slideDashDuration;


        _capsuleCollider.height = _slideDashColliderHeight;
        _capsuleCollider.center = new Vector3(0f, _slideDashColliderHeight / 2f, 0f);

        _rb.AddForce(_slideDashDirection * _slideDashInitialSpeed, ForceMode.Impulse);
    }

    private void SlideDash()
    {

        if (_slideDashTimer > 0f)
        {
            _slideDashTimer -= Time.fixedDeltaTime;

            _rb.AddForce(_slideDashDirection * _slideDashDeceleration * Time.fixedDeltaTime, ForceMode.Acceleration);

            float speedReduction = Mathf.Lerp(_slideDashInitialSpeed, 0f, 1 - (_slideDashTimer / _slideDashDuration));

            _rb.AddForce(_slideDashDirection * speedReduction * Time.fixedDeltaTime, ForceMode.Acceleration);


            if (_rb.velocity.magnitude < 0.1f)
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


    private void ResetRotation()
    {
        //Note: the reset rotation function does not work because if you rotate the player the camera rotates with the player instead of staying loose from it and not rotating with it
        float camTargetRotationX = _cameraController.MainCam.transform.rotation.y;
        float camTargetRotationY = _cameraController.MainCam.transform.rotation.y;
        float camTargetRotationZ = _cameraController.MainCam.transform.rotation.y;

        float myTargetRotationX = 0f;
        float myTargetRotationY = _cameraController.MainCam.transform.rotation.y;
        float myTargetRotationZ = 0f;
        Vector3 myEulerAngleRotation = new Vector3(myTargetRotationX, myTargetRotationY, myTargetRotationZ);
        _rb.transform.rotation = Quaternion.Euler(myEulerAngleRotation);

        Vector3 camEulerAngleRotation = new Vector3(camTargetRotationX, camTargetRotationY, camTargetRotationZ);
        _cameraController.MainCam.transform.rotation = Quaternion.Euler(camEulerAngleRotation);
        Debug.Log("reset rotation" + camEulerAngleRotation);

    }
    #endregion

    #region Dashing
    void PlayerDash()
    {

        if (!_isDashing && _canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        _audioSource.clip = dashSound;
        _audioSource.Play();
        _canDash = false;
        _isDashing = true;
        _dashDirection = _rb.transform.forward;
        float startTime = Time.time;

        while (Time.time < startTime + _dashTime)
        {
            _rb.AddForce(_dashDirection * _dashSpeed, ForceMode.Impulse);

            yield return null; // Wait for next frame
        }
        _isDashing = false;

    }
    #endregion
}
