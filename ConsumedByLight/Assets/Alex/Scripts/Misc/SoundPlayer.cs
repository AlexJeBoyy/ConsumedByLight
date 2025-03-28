using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource reloadSound;

    public void ReloadSound()
    {
        reloadSound.Play();
    }
}
