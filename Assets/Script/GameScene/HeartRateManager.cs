using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class heartRateManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bpmText;
    [SerializeField] TextMeshProUGUI stressReachedText;
    public Image heartRateBar;
    private float bpmFactor;
    private float bpmDiff;
    private float currentFillAmount;
    private float lastHardWaveTime = 0f;
    private float hardWaveCooldown = 10f;
    private bool hardWave = false;
    private float HR_1min;
    private float HRV_1min;
    private float HR_10s;
    float HRV_10s;
    float timpBpm;

    void Start()
    {
        Application.targetFrameRate = 60;
        bpmDiff = 0f;
        bpmFactor = 0.01f;
        currentFillAmount = 0f;
        Debug.Log("currentFillAmount : " + currentFillAmount);
        timpBpm = Time.time; 
    }

    void Update()
    {

        if (UDPListener.instance != null)
        {
            
            if(!WaveManager.instance.WaveRunning()) return;

            bpmText.text = HR_10s.ToString() + " BPM";
            HR_1min = 80;
            HR_10s = 90;
            if(Time.time - timpBpm > 10f)
            {
                // HR_10s = UnityEngine.Random.Range(70, 90);
                timpBpm = Time.time;
            }

            // HR_1min = UDPListener.instance.getHR_1min();
            // HRV_1min = UDPListener.instance.getHRV_1min();
            // HR_10s = UDPListener.instance.getHR_10s();
            // HRV_10s = UDPListener.instance.getHRV_10s();

            // Debug.Log("Current FPS: " + (1.0f / Time.deltaTime));
            // Debug.Log("HR 1min: " + HR_1min);
            // Debug.Log("HRV 1min: " + HRV_1min);
            // Debug.Log("HR 10s: " + HR_10s);
            // Debug.Log("HRV 10s: " + HRV_10s);

            

            bpmDiff = Math.Max(0, HR_10s - HR_1min);

            if (HR_1min > 0)
            {
                bpmFactor = Math.Min(bpmDiff / (2f * HR_1min), 1f)/(1.0f / Time.deltaTime); 
            }
            else
            {
                bpmFactor = 0f; // Évite NaN en cas de HR_1min = 0
            }

            // Debug.Log("bpmDiff : " + bpmDiff);
            // Debug.Log("bpmFactor : " + bpmFactor);

            if (!hardWave)
            {
                currentFillAmount += bpmFactor;
                // Debug.Log("currentFillAmount : " + currentFillAmount);
            }

            if (currentFillAmount >= 1f && Time.time - lastHardWaveTime >= hardWaveCooldown && !hardWave)
            {

                currentFillAmount = 1f;
                StressReached();
                WaveManager.instance.WaveStress();
                lastHardWaveTime = Time.time;
            }

            heartRateBar.fillAmount = currentFillAmount;

        }
        else
        {
            Debug.LogError("UDPListener n'est pas initialisé!");
        }

    }

    void StressReached()
    {
        // Si le joueur atteint un stress maximum, on affiche une barre rouge
        heartRateBar.color = Color.red;
        hardWave = true;
        stressReachedText.gameObject.SetActive(true);
        Invoke("EndStressReached", 10f);
    }

    public void EndStressReached()
    {
        currentFillAmount = 0f;
        heartRateBar.color = Color.white;
        stressReachedText.gameObject.SetActive(false);
        hardWave = false;
    }



}
