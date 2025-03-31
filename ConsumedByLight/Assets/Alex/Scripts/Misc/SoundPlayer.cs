using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource reloadSound;
    public AudioSource crossEnemy;
    public AudioClip crossSmash;
    public void ReloadSound()
    {
        reloadSound.Play();
    }
    public void CrossSmashSound()
    {
        crossEnemy.clip = crossSmash;
        crossEnemy.Play();
    }
}
