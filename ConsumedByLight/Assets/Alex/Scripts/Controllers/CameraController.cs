using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public bool UsingOrbidCam { get; private set; } = false; 

    [SerializeField] PlayerInput _input;

    CinemachineVirtualCamera _activeCam;
    int _activeCamPriorityModifer = 31337;

    [Header("Camera's")]
    public Camera MainCam;
    public CinemachineVirtualCamera c1Person;
    public CinemachineVirtualCamera c3Person;
    public CinemachineVirtualCamera orbitCam;

    private void Start()
    {
        c1Person.Priority += _activeCamPriorityModifer;
        _activeCam = c1Person;
        FirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    //private void Update()
    //{
    //    if (_input.CamChangePressed)
    //    {
    //        ChangeCam();
    //    }
    //}

    private void ChangeCam()
    {
        if (c3Person == _activeCam)
        {
            SetCameraPriorities(c3Person, c1Person);
            UsingOrbidCam = false; //un needed but handy if you add more cams
            FirstPerson();
        }
        else if (c1Person == _activeCam)
        {
            SetCameraPriorities(c1Person, orbitCam);
            UsingOrbidCam = true;
            MainCam.cullingMask |= (1 << LayerMask.NameToLayer("Player"));
        }
        else if (orbitCam == _activeCam)
        {
            SetCameraPriorities(orbitCam, c3Person);
            _activeCam = c3Person;
            UsingOrbidCam = false;
        }
        else
        {
            c1Person.Priority += _activeCamPriorityModifer;
            _activeCam = c1Person;
        }
    }
    private void FirstPerson()
    {
        MainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        //MainCam.cullingMask | = (1 << LayerMask.NameToLayer("Player")); to undo
    }

    private void SetCameraPriorities(CinemachineVirtualCamera CurrentCam, CinemachineVirtualCamera NewCam)
    {
        CurrentCam.Priority -= _activeCamPriorityModifer;
        NewCam.Priority -= _activeCamPriorityModifer;
        _activeCam = NewCam;
    }
}
