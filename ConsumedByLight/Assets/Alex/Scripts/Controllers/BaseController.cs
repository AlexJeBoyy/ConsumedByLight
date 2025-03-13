using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] Transform CameraFollow;
    [SerializeField] PlayerInput _input;
    Rigidbody _rigidbody = null;
    CapsuleCollider _capsuleCollider = null;

    Vector3 _playerMoveInput;

    Vector3 _playerLookInput;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    [SerializeField] float _cameraPitch = 0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30f;
    [SerializeField] float _rotationSpeedMultiplier = 180f;
    [SerializeField] float _pitchSpeedMultiplier = 180f;
    [SerializeField] float _runMultiplier = 2.5f;

    [Header("Ground Check")]
    [SerializeField] bool _playerIsGrounded = true;
    [SerializeField][Range(0f, 1.8f)] float _groundCheckRaduisMultiplier = .9f;
    [SerializeField][Range(-.95f, 1.05f)] float _groundCheckDistance = .05f;
    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float _gravityFallCurrent = -100f;
    [SerializeField] float _gravityFallMin = -100f;
    [SerializeField] float _gravityFallMax = -500f;
    [SerializeField][Range(-5f, -35f)] float _gravityFallIncrementAmount = -20f;
    [SerializeField] float _gravityFallIncrementTime = -.05f;
    [SerializeField] float _playerFallTimer = -0f;
    [SerializeField] float _gravityGrounded = -1f;
    [SerializeField] float _maxSlopeAngle = -47.5f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        _playerLookInput = GetLookInput();
        PlayerLook();
        PitchCamera();

        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerIsGroundedCheck();

        _playerMoveInput = PlayerMove();
        _playerMoveInput = PlayerRun();
        _playerMoveInput = PlayerSlope();
        _playerMoveInput.y = PlayerFallGravity();

        _playerMoveInput *= _rigidbody.mass; //Note: Dev purposes

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }
    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0f);
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
    private Vector3 PlayerMove()
    {

        return new Vector3 (_playerMoveInput.x * _movementMultiplier,
                            _playerMoveInput.y * _rigidbody.mass,
                            _playerMoveInput.z * _movementMultiplier);
        
    }
    private bool PlayerIsGroundedCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRaduisMultiplier;
        float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y - sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_rigidbody.position, sphereCastRadius, Vector3.down, out _groundCheckHit, sphereCastTravelDistance);
    }
    private Vector3 PlayerSlope()
    {
        Vector3 calculatePlayerMovement = _playerMoveInput;
        if (_playerIsGrounded)
        {
            Vector3 localGroundCheckHitNormal = _rigidbody.transform.InverseTransformDirection(_groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rigidbody.transform.up);
            if (groundSlopeAngle == 0.0f)
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundCheckHitNormal);
                calculatePlayerMovement = slopeAngleRotation * calculatePlayerMovement;
            }
#if UNITY_EDITOR
            Debug.DrawRay(_rigidbody.position, _rigidbody.transform.TransformDirection(calculatePlayerMovement), Color.red, 0.5f);
#endif
        }
        return calculatePlayerMovement;
    }

    private float PlayerFallGravity()
    {
        float gravity = _playerMoveInput.y;
        if (_playerIsGrounded)
        {
            _gravityFallCurrent = _gravityFallMin;// Reset
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

    private Vector3 PlayerRun()
    {
        Vector3 calculatePlayerRunSpeed = _playerMoveInput;
        if (_input.RunIsPressed //*&& _input.MoveIsPressed*//
                                )
        {
            calculatePlayerRunSpeed.x *= _runMultiplier;
            calculatePlayerRunSpeed.z *= _runMultiplier;
        }
        return calculatePlayerRunSpeed;
    }
  
}
