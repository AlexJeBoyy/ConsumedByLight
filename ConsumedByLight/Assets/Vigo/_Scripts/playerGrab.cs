using System.Collections;
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
    [SerializeField] private Slider staminaGrab;
    [SerializeField] private Slider staminaGrab2;
    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;
    private bool usingStamina;

    Outline currentOutline;


    Rigidbody grabbedRB;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        if (grabbedRB)
        {
            usingStamina = true;
            float distanceFromCamera = Vector3.Distance(cam.transform.position, objectHolder.position);
            targetPosistion = cam.transform.position + cam.transform.forward * distanceFromCamera;

            float dist = Vector3.Distance(targetPosistion, grabbedRB.transform.position);
            if (dist > maxdist)
            {
                if (grabbedRB.gameObject.GetComponent<NavMeshAgent>() != null)
                {
                    grabbedRB.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                }
                grabbedRB.useGravity = true;
                grabbedRB.freezeRotation = false;
                grabbedRB = null;
                usingStamina = false;
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

    private void Update()
    {
        staminaGrab.value = currentStamina;
        staminaGrab2.value = currentStamina;

        if (usingStamina)
        {
            currentStamina = currentStamina - Time.deltaTime * 10;
        }

        if (currentStamina <= 0f && grabbedRB != null)
        {
            if (grabbedRB.gameObject.GetComponent<NavMeshAgent>() != null)
            {
                grabbedRB.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            grabbedRB.useGravity = true;
            grabbedRB.freezeRotation = false;
            grabbedRB = null;
            usingStamina = false;
        }

        if (grabbedRB == null && currentStamina <= 100)
        {
            StartCoroutine(StaminaRecharge());
        }
        else
        {
            StopAllCoroutines();
        }

        CheckForOutline();
    }

    void CheckForOutline()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out hit, maxGrabDistance))
        {
            Outline newOutline = hit.collider.GetComponentInChildren<Outline>();
            if (newOutline != null)
            {
                if (newOutline != currentOutline || currentOutline == null)
                {
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                    }
                    currentOutline = newOutline;
                    currentOutline.enabled = true;
                    Debug.Log(newOutline);
                }
                else
                {
                    currentOutline.enabled = true;
                }
            }
            else
            {
                if (currentOutline != null)
                {
                    currentOutline.enabled = false;
                }
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
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
            usingStamina = false;
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
        if (ctx.performed && grabbedRB != null)
        {
            isCharging = true;
        }
        else if (ctx.canceled && grabbedRB != null)
        {
            grabbedRB.AddForce(cam.transform.forward * throwForce * chargeTime / maxChargeTime, ForceMode.Impulse);
            currentStamina = currentStamina - 25;
            isCharging = false;
            chargeTime = 0f;
            grabbedRB.useGravity = true;
            grabbedRB = null;
            usingStamina = false;
        }
    }

    private IEnumerator StaminaRecharge()
    {
        yield return new WaitForSeconds(2);
        currentStamina = currentStamina + Time.deltaTime * 10;
    }
}
