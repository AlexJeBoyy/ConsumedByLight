using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject bloodVfx;
    [SerializeField] BaseController move;
    [SerializeField] PlayerGrab grab;
    [SerializeField] Animator deathAnim;
    [SerializeField] GameObject gun;
    [SerializeField] float health;
    bool dead = false;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0 && !dead)
        {
            grab.enabled = false;
            move.enabled = false;
            dead = true;
            gun.SetActive(false);
            deathAnim.Play("Die");
        }
        Instantiate(bloodVfx, gameObject.transform.position, Quaternion.identity);
    }
}
