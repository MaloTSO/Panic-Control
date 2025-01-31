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
        timeText.text = "30";
        waveText.text = "Wave 1";

    }

    private void Update()
    {
        zombieCount = EnemyManager.instance.GetZombieCount();
        //for testing
        if (zombieCount == 0)
        {
            zombieCountText.color = Color.green; 
            zombieCountText.text = "Press Space to start next wave";
            if(Input.GetKeyDown(KeyCode.Space)){
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
        EnemyManager.instance.StartSpawning();
        StopAllCoroutines();
        timeText.color = Color.white;
        currentWave++;
        waveRunning = true;
        currentWaveTime = 30;
        waveText.text = "Wave " + currentWave;
        StartCoroutine(WaveTimer());
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
        currentWaveTime = 30;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
        waveText.text = "Kill the remaining zombies";
        HRM.EndStressReached();
    }

    public void IncreaseDifficulty()
    {
        //pour augmenter la difficulté au fil des wave
    }

    public void HardWave()
    {
        
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.instance.SpawnEnemy();
        }

    }
}
