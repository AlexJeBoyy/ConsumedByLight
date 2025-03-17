using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestDamageDealer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<Animator>().Play("pushed");
        }
    }
}
