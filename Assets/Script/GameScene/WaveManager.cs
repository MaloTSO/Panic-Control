using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;

    public static WaveManager instance;

    int currentWave = 0;
    bool waveRunning = true;
    int currentWaveTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartNewWave();
        timeText.text = "30";
        waveText.text = "Wave 1";
    }

    private void Update(){
        //for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNewWave();
        }
    }

    private void StartNewWave()
    {
        StopAllCoroutines();
        timeText.color = Color.white;
        currentWave++;
        waveRunning = true;
        currentWaveTime = 30;
        waveText.text = "Wave " + currentWave;
        StartCoroutine(WaveTimer());
    }

    public bool WaveRunning() => waveRunning;

    private IEnumerator WaveTimer(){
        //permet d'executer cette fonction toutes les secondes et sans bloquer le jeu garce à StartCoroutine
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;
            timeText.text = currentWaveTime.ToString();
            if (currentWaveTime <= 0)
            {
                WaveComplete();
            }
        }
        yield return null; 
    }

    private void WaveComplete(){
        StopAllCoroutines();
        EnemyManager.instance.DestroyAllEnemies();
        waveRunning = false;
        currentWaveTime = 30;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
    }
}
