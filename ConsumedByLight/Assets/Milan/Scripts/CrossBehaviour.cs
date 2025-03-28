using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrossBehaviour : MonoBehaviour
{
    [SerializeField] GameObject smashVFX;
    [SerializeField] Transform impactPoint;
    [SerializeField] float explosiveRadius;
    [SerializeField] LayerMask layerMask;

    public void SpawnImpact()
    {
        Instantiate(smashVFX, impactPoint.position, Quaternion.Euler(-90,0,0));
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosiveRadius, layerMask);

        foreach (Collider hit in hitObjects)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                hit.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
    }
}
