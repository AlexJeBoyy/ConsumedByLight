using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    Rigidbody _rigidbody = null;
    [SerializeField] PlayerInput _input;

    Vector3 _playerMoveInput;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30f;

    private void Awake()
    {
        _rigidbody= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _playerMoveInput = GetMoveInput();
        PlayerMove();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private void PlayerMove()
    {
        
        _playerMoveInput = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass,
                                        _playerMoveInput.y,
                                        _playerMoveInput.z * _movementMultiplier * _rigidbody.mass));
        //Debug.Log(_playerMoveInput);
    }
}
