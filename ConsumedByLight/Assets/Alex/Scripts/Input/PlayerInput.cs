using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;

    InputActions _input = null;

    private void OnEnable()
    {
        _input = new InputActions();
        _input.BaseGameplay.Enable();

        _input.BaseGameplay.Move.started += SetMove;
        _input.BaseGameplay.Move.performed += SetMove;
        _input.BaseGameplay.Move.canceled += SetMove;

        _input.BaseGameplay.Look.performed += SetMove;
        _input.BaseGameplay.Look.canceled += SetMove;
    }

    private void OnDisable()
    {
        _input.BaseGameplay.Move.performed -= SetLook;
        _input.BaseGameplay.Move.canceled -= SetLook;

        _input.BaseGameplay.Look.performed -= SetLook;
        _input.BaseGameplay.Look.canceled -= SetLook;

        _input.BaseGameplay.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
       MoveInput = ctx.ReadValue<Vector2>();
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
}
