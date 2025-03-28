using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestDamageDealer : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject healVfx;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<Animator>().Play("pushed");
        }

        if (other.gameObject.CompareTag("Paladin"))
        {
            Debug.Log("Touched a paladin");
            //other.gameObject.GetComponent<> <- HEAL
            Instantiate(healVfx, other.transform.position, Quaternion.Euler(-90, 0, 0));
        }
    }
}
