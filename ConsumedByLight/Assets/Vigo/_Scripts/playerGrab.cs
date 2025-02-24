using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 10f, throwForce = 20f, lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;

    Rigidbody grabbedRB;

    public void Grab()
    {
        if (grabbedRB)
        {

        }
        else
        {

        }
    }
}
