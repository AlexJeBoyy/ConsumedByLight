using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    [Header("Weapon Sway")]
    [SerializeField] private float posistionalSway = 0.1f;
    [SerializeField] private float rotationalSway = 0.1f;
    [SerializeField] private float swaySmoothness;

    private Vector3 initialPosistion = Vector3.zero;
    private Quaternion initialRotation = Quaternion.identity;

    [Header("Weapon Bobbing")]
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingAmount = 5f;

    private float bobTimer = 0f;
    public BaseController player;

    [Header("Weapon Recoil")]

    [SerializeField] private float recoilAmount = 0.2f;
    [SerializeField] private float recoilSmoothness = 5f;

    [HideInInspector] public bool isRecoiling = false;
    private Vector3 currentrecoil = Vector3.zero;

    private void Start()
    {
        initialPosistion = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        ApplySway();
        ApplyBobbing();
        ApplyRecoil();
    }

    private void ApplySway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 positionOffset = new Vector3(mouseX, mouseY, 0) * posistionalSway;
        Quaternion rotationOffset = Quaternion.Euler(new Vector3(-mouseY, mouseX, mouseX) * rotationalSway);

        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosistion - positionOffset, Time.deltaTime * swaySmoothness);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation * rotationOffset, Time.deltaTime * swaySmoothness);
    }

    private void ApplyBobbing()
    {
        float moveSpeed = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        float bobOffset = 0f;

        if (moveSpeed > 0.1f && player._playerIsGrounded)
        {
            bobTimer += Time.deltaTime * bobbingSpeed;
            bobOffset = Mathf.Sin(bobTimer) * bobbingAmount;
        }
        else
        {
            bobTimer = 0f;
            bobOffset = Mathf.Lerp(bobTimer, 0, Time.deltaTime * swaySmoothness);
        }

        transform.localPosition += new Vector3(0, bobOffset, 0);
    }

    private void ApplyRecoil()
    {
        Vector3 targetRecoil = Vector3.zero;

        if (isRecoiling)
        {
            targetRecoil = new Vector3(0, 0, -recoilAmount);

            if (Vector3.Distance(currentrecoil, targetRecoil) < 0.1f)
            {
                isRecoiling = false;
            }
        }

        currentrecoil = Vector3.Lerp(currentrecoil, targetRecoil, Time.deltaTime * recoilSmoothness);
        transform.localPosition -= currentrecoil;
    }
}
