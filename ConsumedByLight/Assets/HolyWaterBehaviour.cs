using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterBehaviour : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject impactCheck;
    [SerializeField] float explosiveRadius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject healVfx;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitParticle, transform.position, Quaternion.Euler(-90, 0 ,0));
        CheckHit();
    }

    private void CheckHit()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosiveRadius, layerMask);

        foreach (Collider hit in hitObjects)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                //hit.GetComponent<Enemy>().TakeDamage();
                hit.gameObject.GetComponentInChildren<Animator>().Play("pushed");
            }
            else
            {
                Instantiate(healVfx, new Vector3(hit.transform.position.x, hit.transform.position.y - 1.2f, hit.transform.position.z), Quaternion.Euler(-90, 0, 0));
            }
        }

        Destroy(gameObject);
    }
}
