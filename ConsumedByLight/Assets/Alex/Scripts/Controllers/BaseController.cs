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
    [SerializeField] float _gravity = -0f;
    private void Awake()
    {
        _rigidbody= GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>(); 
    }

    private void FixedUpdate()
    {
        _playerLookInput = GetLookInput();
        PlayerLook();
        PitchCamera();

        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerIsGroundedCheck();
        _playerMoveInput.y = PlayerGravity();

        _playerMoveInput = PlayerMove();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput= _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }
    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0f,_rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier),0f);
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

    private float PlayerGravity()
    {
        if (_playerIsGrounded)
        {
            _gravity = 0.0f;
            _gravityFallCurrent = _gravityFallMin;
        }
        else
        {
            _playerFallTimer -= Time.fixedDeltaTime;
            if (_playerFallTimer < 0.0f)
            {
                if(_gravityFallCurrent > _gravityFallMax)
                {
                    _gravityFallCurrent += _gravityFallIncrementAmount;
                }
                _playerFallTimer = _gravityFallIncrementTime;
                _gravity = _gravityFallCurrent;
            }
        }
        return _gravity;
    }
    private Vector3 PlayerMove()
    {
        
        Vector3 calcPlayerMovement = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass,
                                        _playerMoveInput.y * _rigidbody.mass,
                                        _playerMoveInput.z * _movementMultiplier * _rigidbody.mass));
        return calcPlayerMovement;
    }
}
