using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{   void Start()
    {
        Invoke("KillParticle", 5);
    }

    void KillParticle()
    {
        Destroy(gameObject);
    }
}
