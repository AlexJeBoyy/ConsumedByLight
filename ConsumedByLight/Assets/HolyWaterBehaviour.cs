using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterBehaviour : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject impactCheck;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitParticle, transform.position, Quaternion.Euler(-90, 0 ,0));
        StartCoroutine(CheckCol());
    }

    IEnumerator CheckCol()
    {
        impactCheck.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        impactCheck.SetActive(false);
        Destroy(gameObject);
    }
}
