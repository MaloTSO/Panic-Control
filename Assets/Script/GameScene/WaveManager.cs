using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI zombieCountText;

    private heartRateManager HRM;
    public static WaveManager instance;

    int currentWave = 0;
    bool waveRunning = true;
    int currentWaveTime;
    public bool hardWave = false;
    private int zombieCount;
    private bool isCalibrated = false;
    private int calibrationWave = 1;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        HRM = FindObjectOfType<heartRateManager>();

        StartNewWave();

    }

    private void Update()
    {
        zombieCount = EnemyManager.instance.GetZombieCount();
        //for testing
        if (zombieCount == 0 && currentWaveTime == 0)
        {
            zombieCountText.color = Color.green;
            zombieCountText.text = "Press Space to start next wave";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartNewWave();
            }
        }
        else
        {
            zombieCountText.color = Color.white;
            zombieCountText.text = "Zombies remaining: " + zombieCount;
        }
    }

    private void StartNewWave()
    {
        currentWaveTime = 30;
        // if (!isCalibrated && calibrationWave == 1)
        // {
        //     Calibration(calibrationWave);
        //     waveText.text = "Easy calibration Waves";
        // }
        // else if (!isCalibrated && calibrationWave == 2)
        // {
        //     Calibration(calibrationWave);
        //     waveText.text = "Medium calibration Waves";
        // }
        // else if (!isCalibrated && calibrationWave == 3)
        // {
        //     Calibration(calibrationWave);
        //     waveText.text = "Hard calibration Waves";
        // }
        // else
        // {
            EnemyManager.instance.StartSpawning();
            StopAllCoroutines();
            timeText.color = Color.white;
            currentWave++;
            waveRunning = true;
            currentWaveTime = 30;
            waveText.text = "Wave " + currentWave;
            StartCoroutine(WaveTimer());
        // }

    }

    public bool WaveRunning() => waveRunning;

    private IEnumerator WaveTimer()
    {
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

    private void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.instance.StopSpawning();
        waveRunning = false;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
        waveText.text = "Kill the remaining zombies";
        HRM.EndStressReached();
    }

    public void IncreaseDifficulty()
    {
        //pour augmenter la difficulté au fil des wave
    }

    public void WaveStress()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnEnemy();
        }
    }


    public void Calibration(int wave)
    {
        StopAllCoroutines();
        timeText.color = Color.white;
        waveRunning = true;
        currentWaveTime = 30;
        StartCoroutine(WaveTimer());
        if (wave == 1)
        {
            Debug.Log("Calibration wave 1");
            EasyWave();
        }
        else if (wave == 2)
        {
            Debug.Log("Calibration wave 2");
            MediumWave();
        }
        else if (wave == 3)
        {
            Debug.Log("Calibration wave 3");
            HardWave();
            isCalibrated = true;
        }
        calibrationWave++;
    }

    private void SpawnEnemyDelayed()
    {
        EnemyManager.instance.SpawnEnemy();
    }

    private void SpawnChargerDelayed()
    {
        EnemyManager.instance.SpawnCharger();
    }

    private void SpawnBossDelayed()
    {
        EnemyManager.instance.SpawnBoss();
    }

    public void EasyWave()
    {
        EnemyManager.instance.StartSpawning();
        for (int i = 0; i < 5; i++)
        {
            EnemyManager.instance.SpawnEnemy();
            Invoke("SpawnChargerDelayed", 3f * (i + 1));
        }
    }

    public void MediumWave()
    {
        EnemyManager.instance.StartSpawning();
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnEnemy();
            Invoke("SpawnEnemyDelayed", 2f * (i + 1));
        }
        Invoke("SpawnBossDelayed", 20f);
    }

    public void HardWave()
    {
        EnemyManager.instance.StartSpawning();
        StartCoroutine(SpawnHardWave());
    }

    private IEnumerator SpawnHardWave()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnEnemy();
            yield return null;
        }


        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(5f);
            EnemyManager.instance.SpawnCharger();
        }

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(20f);
            EnemyManager.instance.SpawnBoss();
        }
    }

}
