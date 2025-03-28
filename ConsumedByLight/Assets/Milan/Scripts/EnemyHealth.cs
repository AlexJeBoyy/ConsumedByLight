using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject bloodVfx;
    public void TakeDamage(Vector3 damagePoint)
    {
        GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        Vector3 dir = transform.position - damagePoint;
        dir.Normalize();
        rb.AddForce(dir* 50, ForceMode.Impulse);
        Instantiate(bloodVfx, damagePoint, Quaternion.identity);
        Destroy(gameObject, 1f);
    }
}
