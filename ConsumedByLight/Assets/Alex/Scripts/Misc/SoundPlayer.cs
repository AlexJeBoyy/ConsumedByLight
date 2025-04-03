using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource reloadSound;
    public AudioSource audioSource;
    public AudioClip crossSmash;
    public AudioClip paladinSwing;
    public AudioClip waveChage;
    public void ReloadSound()
    {
        reloadSound.Play();
    }
    public void CrossSmashSound()
    {
        audioSource.clip = crossSmash;
        audioSource.Play();
    }

    public void PaladinSwingSound()
    {
        audioSource.clip = paladinSwing;
        audioSource.Play();
    }

    public void WaveChange()
    {
        audioSource.clip = waveChage;
        audioSource.Play();
    }
}
