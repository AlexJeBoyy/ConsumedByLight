using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectColor : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 0.75f, 1f, 0.5f, 1f);
    }
}
