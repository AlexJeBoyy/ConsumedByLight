using UnityEngine;

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

            grabbedRB.freezeRotation = true;

            grabbedRB.transform.position = Vector3.Lerp(grabbedRB.transform.position, targetPosistion, lerpSpeed * Time.deltaTime);


            objectHolder.transform.position = objectHolder.transform.position + cam.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            distanceFromCamera = Mathf.Clamp(distanceFromCamera, 1f, maxGrabDistance);
        }
    }
    public void Grab()
    {
        if (grabbedRB)
        {
            Vector3 horizontalVelocity = new Vector3(grabbedRB.velocity.x, 0, grabbedRB.velocity.z);
            Vector3 horizontalDirection = horizontalVelocity.normalized;
            if (horizontalDirection == Vector3.zero)
            {
                horizontalDirection = cam.transform.forward;
            }
            grabbedRB.AddForce(horizontalDirection * dropForce, ForceMode.VelocityChange);
            grabbedRB.freezeRotation = false;
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

    public void Throw()
    {
        if (grabbedRB)
        {
            grabbedRB.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
            grabbedRB.useGravity = true;
            grabbedRB.freezeRotation = false;
            grabbedRB = null;
        }
    }
}
