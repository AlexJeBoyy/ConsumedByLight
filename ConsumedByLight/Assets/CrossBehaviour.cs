using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrossBehaviour : MonoBehaviour
{
    [SerializeField] GameObject smashVFX;
    [SerializeField] Transform impactPoint;

    public void SpawnImpact()
    {
        Instantiate(smashVFX, impactPoint.position, Quaternion.Euler(-90,0,0));
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.gameObject.GetComponentInChildren<Animator>().Play("pushed");
    //    }
    //}
}
