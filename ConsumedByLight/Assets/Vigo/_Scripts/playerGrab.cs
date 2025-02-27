using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 10f, throwForce = 20f, dropForce = 5f, lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;
    [SerializeField] float scrollSpeed = 250f;
    private Vector3 targetPosistion;

    Rigidbody grabbedRB;

    private void Update()
    {
        if (grabbedRB)
        {


            float distanceFromCamera = Vector3.Distance(cam.transform.position, objectHolder.position);
            targetPosistion = cam.transform.position + cam.transform.forward * distanceFromCamera;


            grabbedRB.transform.position = Vector3.Lerp(grabbedRB.transform.position, targetPosistion, lerpSpeed * Time.deltaTime);


            objectHolder.transform.position = objectHolder.transform.position + cam.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            distanceFromCamera = Mathf.Clamp(distanceFromCamera, 1f, maxGrabDistance);
        }
    }
    public void Grab()
    {
        if (grabbedRB)
        {
            grabbedRB.useGravity = true;
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

    public void Throw(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && grabbedRB)
        {
            grabbedRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
            grabbedRB.useGravity = true;
            grabbedRB.freezeRotation = false;
            grabbedRB = null;
        }
    }
}
