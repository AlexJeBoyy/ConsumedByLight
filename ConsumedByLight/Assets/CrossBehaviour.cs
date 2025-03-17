using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrossBehaviour : MonoBehaviour
{
    [SerializeField] GameObject smashVFX;
    [SerializeField] Transform impactPoint;
    [SerializeField] GameObject colCheck;

    public void SpawnImpact()
    {
        Instantiate(smashVFX, impactPoint.position, Quaternion.Euler(-90,0,0));
        StartCoroutine(CheckCol());
    }
    
    IEnumerator CheckCol()
    {
        colCheck.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        colCheck.SetActive(false);
    }
}
