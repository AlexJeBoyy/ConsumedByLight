using System.Collections;
using UnityEngine;

public class EnemySmash : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speedOfObject;
    [SerializeField] private float destroySpeedThreshold = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        speedOfObject = rb.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (speedOfObject >= destroySpeedThreshold)
        {
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}