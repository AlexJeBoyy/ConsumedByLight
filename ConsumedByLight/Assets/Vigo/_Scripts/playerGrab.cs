using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{

    [Header("Player Camera")]
    [SerializeField] Camera cam;


    [Header("Object Attributes")]
    [SerializeField] float maxGrabDistance = 10f;
    [SerializeField] float throwForce = 20f;
    [SerializeField] float lerpSpeed = 10f;


    [Header("Holder Attributes")]
    [SerializeField] Transform objectHolder;
    [SerializeField] float scrollSpeed = 250f;
    [SerializeField] float followDeadzone = 0.1f;
    private Vector3 targetPosistion;
    [SerializeField] private float distanceMod;
    [SerializeField] private float maxdist;

    [Header("Charge")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeTime;
    [SerializeField] private float maxChargeTime;
    private bool isCharging;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlide;

    Rigidbody grabbedRB;

    private void FixedUpdate()
    {
        if (grabbedRB)
        {


            float distanceFromCamera = Vector3.Distance(cam.transform.position, objectHolder.position);
            targetPosistion = cam.transform.position + cam.transform.forward * distanceFromCamera;

            float dist = Vector3.Distance(targetPosistion, grabbedRB.transform.position);
            if (dist > maxdist)
            {
                grabbedRB.useGravity = true;
                grabbedRB = null;
                return;
            }
            if (dist < followDeadzone)
            {
                grabbedRB.velocity = Vector3.zero;
            }
            else
            {
                Vector3 direction = (targetPosistion - grabbedRB.transform.position).normalized;
                grabbedRB.velocity = (direction * Time.deltaTime * lerpSpeed * (dist * distanceMod));
            }

            // grabbedRB.transform.position = Vector3.Lerp(grabbedRB.transform.position, targetPosistion, lerpSpeed * Time.deltaTime);




            objectHolder.transform.position = objectHolder.transform.position + cam.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            distanceFromCamera = Mathf.Clamp(distanceFromCamera, 1f, maxGrabDistance);
            if (isCharging)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
                chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            }
        }
    }
    public void Grab()
    {
        if (grabbedRB)
        {
            if (grabbedRB.gameObject.GetComponent<NavMeshAgent>() != null)
            {
                grabbedRB.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            grabbedRB.useGravity = true;
            grabbedRB.freezeRotation = false;
            grabbedRB = null;
        }
        else
        {
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out hit, maxGrabDistance))
            {
                grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (grabbedRB != null)
                {
                    grabbedRB.useGravity = false;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void ThrowCharge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isCharging = true;
        }
        else if (ctx.canceled)
        {
            Debug.Log(chargeTime);
            grabbedRB.AddForce(cam.transform.forward * throwForce * chargeTime / maxChargeTime, ForceMode.Impulse);
            isCharging = false;
            chargeTime = 0f;
            grabbedRB.useGravity = true;
            grabbedRB = null;
        }
    }
}
