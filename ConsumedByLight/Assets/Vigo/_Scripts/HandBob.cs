using UnityEngine;

public class HandBob : MonoBehaviour
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

    private void Start()
    {
        initialPosistion = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        ApplySway();
        ApplyBobbing();
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
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float moveSpeed = moveInput.magnitude;
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
}
