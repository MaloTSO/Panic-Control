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
    public bool isCalibrated = false;
    private int calibrationWave = 0;


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

        if (PlayerPrefs.GetInt("IsCalibrated", 0) == 1)
        {
            isCalibrated = true;
            calibrationWave = 4;
        }

        StartNewWave();

    }

    private void Update()
    {
        zombieCount = EnemyManager.instance.GetZombieCount();

        if (zombieCount == 0 && currentWaveTime == 0)
        {
            zombieCountText.color = Color.green;
            zombieCountText.text = "Press Space to start next wave";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (calibrationWave == 3)
                {
                    isCalibrated = true;
                }
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
        StopAllCoroutines();
        currentWaveTime = 30;
        calibrationWave++;
        waveRunning = true;
        if (!isCalibrated && calibrationWave == 1)
        {
            Calibration(calibrationWave);
            waveText.text = "Easy calibration Wave";
            waveText.color = Color.green;
        }
        else if (!isCalibrated && calibrationWave == 2)
        {
            Calibration(calibrationWave);
            waveText.text = "Medium calibration Wave";
            waveText.color = Color.yellow;
        }
        else if (!isCalibrated && calibrationWave == 3)
        {
            Calibration(calibrationWave);
            waveText.text = "Hard calibration Wave";
            waveText.color = Color.red;
        }
        else
        {
            EnemyManager.instance.IncreaseDifficulty(currentWave);
            EnemyManager.instance.StartSpawningWave();
            waveText.color = Color.white;
            timeText.color = Color.white;
            currentWave++;
            currentWaveTime = 30;
            waveText.text = "Wave " + currentWave;
            StartCoroutine(WaveTimer());
        }

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
        waveText.color = Color.white;
        HRM.EndStressReached();
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

            // calibration terminée
            PlayerPrefs.SetInt("IsCalibrated", 1);
            PlayerPrefs.Save();
        }
    }

    private void SpawnEnemyDelayed()
    {
        EnemyManager.instance.SpawnBasicEnemy();
    }

    public void EasyWave()
    {
        EnemyManager.instance.StartSpawning();
    }

    public void MediumWave()
    {
        EnemyManager.instance.StartSpawning();
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnBasicEnemy();
            Invoke("SpawnEnemyDelayed", 2f * (i + 1));
        }
    }

    public void HardWave()
    {
        EnemyManager.instance.StartSpawning();
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnChargers());
        StartCoroutine(SpawnBosses());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnEnemy();
            yield return null;
        }
    }

    private IEnumerator SpawnChargers()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(5f);
            EnemyManager.instance.SpawnCharger();
        }
    }

    private IEnumerator SpawnBosses()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(20f);
            EnemyManager.instance.SpawnBoss();
        }
    }

    public int GetcalibrationWave() => calibrationWave;

}
