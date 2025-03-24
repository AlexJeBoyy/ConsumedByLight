using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.Shapes;

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

    public void ChangeFOV(CinemachineVirtualCamera cam, float endFOV, float duration, float minFOV, float maxFOV)
    {
        cam.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(cam.m_Lens.FieldOfView, endFOV, duration * Time.deltaTime), minFOV, maxFOV);
    }
    //public IEnumerator ChangeFOV(CinemachineVirtualCamera cam, float endFOV, float duration)
    //{
    //    float startFOV = cam.m_Lens.FieldOfView;
    //    float time = 0;
    //    while (time < duration)
    //    {
    //        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, endFOV, Time.deltaTime * 5);
    //        cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
    //        yield return null;
    //        time += Time.deltaTime;
    //    }
        
    //}
}
