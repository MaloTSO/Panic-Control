using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class heartRateManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bpmText;
    [SerializeField] TextMeshProUGUI stressReachedText;
    public Image heartRateBar;
    private float stress_factor;
    private float stress_diff;
    private float currentFillAmount;
    private float lastHardWaveTime = 0f;
    private float hardWaveCooldown = 10f;
    private bool hardWave = false;
    private float stress_100b;
    private float stress_15b;
    private float hr;
    private float stress_ref;

    void Start()
    {
        Application.targetFrameRate = 60;
        stress_diff = 0f;
        stress_factor = 0.01f;
        currentFillAmount = 0f;
        stress_ref = 1.5f;
        Debug.Log("currentFillAmount : " + currentFillAmount);
    }

    void Update()
    {

        if (UDPListener.instance != null)
        {
            
            if(!WaveManager.instance.WaveRunning()) return;

            bpmText.text = hr.ToString() + " BPM";
            stress_100b = 1.5f;
            stress_15b = 2.5f;
            
            stress_100b = UDPListener.instance.stress_level_100b;
            stress_15b = UDPListener.instance.stress_level_15b;
            hr = UDPListener.instance.hr;

            // Debug.Log("stress_100b : " + stress_100b);
            // Debug.Log("stress_15b : " + stress_15b);
            // Debug.Log("hr : " + hr);
            // Debug.Log("Current FPS: " + (1.0f / Time.deltaTime));

            

            stress_diff = Math.Max(stress_100b - stress_15b, 0f);

            if (stress_100b > 0)
            {
                stress_factor = Math.Min(stress_diff / (2f * stress_100b), 1f)/(1.0f / Time.deltaTime); 
            }
            else
            {
                stress_factor = 0f; // Évite NaN en cas de stress_100b = 0
            }

            // Debug.Log("stress_diff : " + stress_diff);
            // Debug.Log("stress_factor : " + stress_factor);

            if (!hardWave)
            {
                currentFillAmount += stress_factor;
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
