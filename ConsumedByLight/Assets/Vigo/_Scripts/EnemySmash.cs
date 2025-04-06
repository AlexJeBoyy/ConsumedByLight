using System.Collections;
using UnityEngine;

public class EnemySmash : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speedOfObject;
    [SerializeField] private float destroySpeedThreshold = 10f;
    [SerializeField] GameObject bloodVfx;

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
            EnemySpawner.instance.numOfAliveEnemies--;
            Instantiate(bloodVfx, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}