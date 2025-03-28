using Cinemachine;
using UnityEngine;

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

    [Header("Shake effect")]
    public float shakeAmount = 0.1f;
    public float shakeDecrease = 1;
    public float _currentShake = 0;

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

    private void Update()
    {
        HandleScreenShake();
    }

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

    private void HandleScreenShake()
    {
        if (_currentShake > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * _currentShake;
            transform.position += shakeOffset;

            _currentShake -= Time.deltaTime * shakeDecrease;
            _currentShake = Mathf.Max(0, _currentShake);
        }
    }

    public void AddShake(float amount)
    {
        _currentShake = Mathf.Max(_currentShake, amount);
    }

    //Note: add this when you want to add the camera shake

    //CamContrl CameraController;

    //if (camContrl != null)
    //{camContrl.AddShake(.2f); //Note: Edit this value to change the intensity of the shake
    //    
    //}


}
