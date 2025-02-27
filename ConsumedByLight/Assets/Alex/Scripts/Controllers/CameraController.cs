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
        ChangeCam();
    }

    private void Update()
    {
        if (_input.CamChangePressed) 
        {
            ChangeCam();
        }
    }

    private void ChangeCam()
    {
        if (c3Person == _activeCam)
        {
            SetCameraPriorities(c3Person, c1Person);
            UsingOrbidCam = false;
        }
        else if (c1Person == _activeCam)
        {
            SetCameraPriorities(c1Person, orbitCam);
            UsingOrbidCam = true;
        }
        else if (orbitCam == _activeCam)
        {
            SetCameraPriorities(orbitCam, c3Person);
            _activeCam = c3Person;
        }
        else
        {
            c1Person.Priority += _activeCamPriorityModifer;
            _activeCam = c1Person;
        }
    }

    private void SetCameraPriorities(CinemachineVirtualCamera CurrentCam, CinemachineVirtualCamera NewCam)
    {
        CurrentCam.Priority -= _activeCamPriorityModifer;
        NewCam.Priority -= _activeCamPriorityModifer;
        _activeCam = NewCam;
    }
}
