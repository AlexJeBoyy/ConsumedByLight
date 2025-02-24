using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 10f, throwForce = 20f, lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;
    [SerializeField] float scrollSpeed = 250f;
    private Vector3 targetPosistion;

    Rigidbody grabbedRB;

    private void Update()
    {
        if (grabbedRB)
        {

            Debug.Log("grab");


            float distanceFromCamera = Vector3.Distance(cam.transform.position, objectHolder.position);
            targetPosistion = cam.transform.position + cam.transform.forward * distanceFromCamera;

            grabbedRB.useGravity = false;

            grabbedRB.transform.position = Vector3.Lerp(grabbedRB.transform.position, targetPosistion, lerpSpeed * Time.deltaTime);


            distanceFromCamera += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
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
            }
        }
    }

    public void Throw()
    {
        if (grabbedRB)
        {
            grabbedRB.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
            grabbedRB.useGravity = true;
            grabbedRB = null;
        }
    }
}
