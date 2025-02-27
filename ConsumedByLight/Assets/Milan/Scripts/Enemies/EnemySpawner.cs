using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject paladinPrefab;
    [SerializeField] int numOfPaladins;
    [SerializeField] float maxRange, minRange;

    private void Start()
    {
        for (int i = 0; i < numOfPaladins; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(minRange, maxRange), 5, Random.Range(minRange, maxRange));
            Instantiate(paladinPrefab, randomPos, Quaternion.identity);
        }
    }

}
