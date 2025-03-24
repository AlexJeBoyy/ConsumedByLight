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
        if (collision.gameObject.CompareTag("Player") && cooldownTime <= 0)
        {
            cooldownTime = cooldownTimer;
            Vector3 collisionPoint = collision.contacts[0].point;
            Instantiate(contactVFX, collisionPoint, Quaternion.identity);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
