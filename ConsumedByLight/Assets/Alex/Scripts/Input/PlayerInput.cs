using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool InvertMouseY { get; private set; } = true;
    public float ZoomCameraInput { get; private set; } = 0f;
    public bool InvertScroll { get; private set; } = true;
    public bool RunIsPressed { get; private set; } = false;
    public bool JumpIsPressed { get; private set; } = false;
    public bool SlideIsPressed { get; private set; } = false;
    public bool DashIsPressed { get; private set; } = false;
    public bool CamChangePressed { get; private set; } = false;
    public bool MoveIsPressed = false;

    InputActions _input = null;
    [SerializeField] CameraController _cameraController;

    private void Update()
    {
        CamChangePressed = _input.BaseGameplay.ChangeCam.WasPressedThisFrame();
    }
    private void OnEnable()
    {

        _input = new InputActions();
        _input.BaseGameplay.Enable();

        //_input.BaseGameplay.Move.started += SetMove;
        _input.BaseGameplay.Move.performed += SetMove;
        _input.BaseGameplay.Move.canceled += SetMove;

        _input.BaseGameplay.Look.performed += SetLook;
        _input.BaseGameplay.Look.canceled += SetLook;

        _input.BaseGameplay.Run.started += SetRun;
        _input.BaseGameplay.Run.canceled += SetRun;

        _input.BaseGameplay.Jump.started += SetJump;
        _input.BaseGameplay.Jump.canceled += SetJump;

        _input.BaseGameplay.Slide.started += SetSlide;
        _input.BaseGameplay.Slide.canceled += SetSlide;

        _input.BaseGameplay.Dash.started += SetDash;
        _input.BaseGameplay.Dash.canceled += SetDash;


    }

    private void OnDisable()
    {
        _input.BaseGameplay.Move.performed -= SetMove;
        _input.BaseGameplay.Move.canceled -= SetMove;

        _input.BaseGameplay.Look.performed -= SetLook;
        _input.BaseGameplay.Look.canceled -= SetLook;

        _input.BaseGameplay.Run.started -= SetRun;
        _input.BaseGameplay.Run.canceled -= SetRun;

        _input.BaseGameplay.Jump.started -= SetJump;
        _input.BaseGameplay.Jump.canceled -= SetJump;

        _input.BaseGameplay.Dash.started -= SetSlide;
        _input.BaseGameplay.Dash.canceled -= SetSlide;

        _input.BaseGameplay.Dash.started -= SetDash;
        _input.BaseGameplay.Dash.canceled -= SetDash;



        _input.BaseGameplay.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        MoveIsPressed = !(MoveInput == Vector2.zero);
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void SetRun(InputAction.CallbackContext ctx)
    {
        RunIsPressed = ctx.started;
    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        JumpIsPressed = ctx.started;
    }

    private void SetSlide(InputAction.CallbackContext ctx)
    {
        SlideIsPressed = ctx.started;
    }
    private void SetDash(InputAction.CallbackContext ctx)
    {
        DashIsPressed = ctx.started;
    }
}
