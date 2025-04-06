using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public int numOfAliveEnemies = 0;

    [SerializeField] GameObject paladinPrefab;

    [SerializeField] GameObject crossEnemyPrefab;

    [SerializeField] GameObject priestEnemyPrefab;

    [SerializeField] float maxRange, minRange;

    [SerializeField] int startWave = 0;

    [SerializeField] Animator waveDisplayer;
    [SerializeField] TextMeshProUGUI wavetext;

    int currentWave = 0;
    bool isSpawning = false;

    [SerializeField] WaveInfo[] WaveData;
    WaveInfo customWave;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentWave = startWave;

        StartCoroutine(SpawnWave(WaveData[currentWave]));
    }

    private void FixedUpdate()
    {
        if (numOfAliveEnemies <= 0 && !isSpawning)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave > WaveData.Length -1)
        {
            customWave.numOfCross = Mathf.RoundToInt(UnityEngine.Random.Range(1, 3) * (1 + (0.5f * currentWave)));
            customWave.numOfPaladins = Mathf.RoundToInt(UnityEngine.Random.Range(3, 6) * (1 + (0.5f * currentWave)));
            StartCoroutine(SpawnWave(customWave));
        }
        else
        {
            StartCoroutine(SpawnWave(WaveData[currentWave]));
        }
    }

    Vector3 GetRandomPos()
    {
        return new Vector3(UnityEngine.Random.Range(minRange, maxRange), 5, UnityEngine.Random.Range(minRange, maxRange));
    }

    IEnumerator SpawnWave(WaveInfo waveInfo)
    {
        isSpawning = true;
        yield return new WaitForSeconds(1f);
        wavetext.text = $"Wave {currentWave + 1}";
        waveDisplayer.Play("WaveDisplayer");

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < waveInfo.numOfPaladins; i++)
        {
            yield return new WaitForSeconds(waveInfo.delayBetweenSpawns);
            numOfAliveEnemies++;
            Instantiate(paladinPrefab, GetRandomPos(), Quaternion.identity);
        }

        for (int i = 0; i < waveInfo.numOfCross; i++)
        {
            yield return new WaitForSeconds(waveInfo.delayBetweenSpawns);
            numOfAliveEnemies++;
            Instantiate(crossEnemyPrefab, GetRandomPos(), Quaternion.identity);
        }

        for (int i = 0; i < waveInfo.numOfPriest; i++)
        {
            yield return new WaitForSeconds(waveInfo.delayBetweenSpawns);
            numOfAliveEnemies++;
            Instantiate(priestEnemyPrefab, GetRandomPos(), Quaternion.identity);
        }
        isSpawning = false;
    }

    [Serializable]
    struct WaveInfo
    {
        public int numOfPaladins;
        public int numOfCross;
        public int numOfPriest;

        public float delayBetweenSpawns;
    }
}
