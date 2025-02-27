using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSwordBehaviour : MonoBehaviour
{
    [SerializeField] GameObject contactVFX;
    [SerializeField] float cooldownTimer = 1;
    float cooldownTime;

    private void Start()
    {
        cooldownTime = 0;
    }

    private void Update()
    {
        cooldownTime -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit something..");
        if (collision.gameObject.CompareTag("Player") && cooldownTime <= 0)
        {
            cooldownTime = cooldownTimer;
            Debug.Log("Hitted the player :)");
            Vector3 collisionPoint = collision.contacts[0].point;
            Instantiate(contactVFX, collisionPoint, Quaternion.identity);
            //Deal damage to player
            collision.gameObject.GetComponentInChildren<Animator>().Play("pushed");
        }
    }
}
