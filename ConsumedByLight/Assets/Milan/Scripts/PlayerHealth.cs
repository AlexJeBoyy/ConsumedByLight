using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject bloodVfx;
    [SerializeField] BaseController move;
    [SerializeField] PlayerGrab grab;
    [SerializeField] Animator deathAnim;
    [SerializeField] Animator UI;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject hand;
    [SerializeField] float health;
    [SerializeField] Slider[] healthSliders;
    bool dead = false;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        foreach (var slider in healthSliders)
        {
            slider.value = health;
        }

        if (health <= 0 && !dead)
        {
            grab.enabled = false;
            move.enabled = false;
            dead = true;
            gun.SetActive(false);
            hand.SetActive(false);
            deathAnim.Play("Die");
            UI.SetTrigger("Show");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        Instantiate(bloodVfx, gameObject.transform.position, Quaternion.identity);
    }
}
