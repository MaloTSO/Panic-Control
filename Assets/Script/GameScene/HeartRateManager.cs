using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class heartRateManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bpmText;
    [SerializeField] TextMeshProUGUI stressReachedText;
    [SerializeField] Image heartRateBar;
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
    private float timetest = 0f;
    private float updateTime = 0f;

    private List<float> easyWaveCalibration = new List<float>();
    private List<float> mediumWaveCalibration = new List<float>();
    private List<float> hardWaveCalibration = new List<float>();



    void Start()
    {
        Application.targetFrameRate = 60;
        stress_diff = 0f;
        stress_factor = 0.01f;
        currentFillAmount = 0f;
        heartRateBar.fillAmount = currentFillAmount;
        stress_ref = 1f;
        updateTime = 0f;
    }

    void Update()
    {
        timetest += Time.deltaTime;
        updateTime += Time.deltaTime;
        if (UDPListener.instance != null)
        {
            // hr = UDPListener.instance.hr;
            if (timetest > 1f)
            {
                hr = UnityEngine.Random.Range(50, 200);
                bpmText.text = hr.ToString() + " BPM";
                // Normalisation de la fréquence cardiaque entre 50 et 200 BPM
                float normalizedBPM = Mathf.InverseLerp(50f, 200f, hr);

                // Interpolation de la couleur entre vert et rouge
                bpmText.color = Color.Lerp(Color.green, Color.red, normalizedBPM);
                timetest = 0f;
            }

            heartRateBar.fillAmount = currentFillAmount;

            Debug.Log("currentFillAmount : " + currentFillAmount);

            if (!WaveManager.instance.WaveRunning()) return;

            // stress_100b = UDPListener.instance.stress_level_100b;
            // stress_15b = UDPListener.instance.stress_level_15b;

            // stress_100b = 1f;
            // stress_15b = 1.5f;

            stress_100b = UnityEngine.Random.Range(0.5f, 3f);
            stress_15b = UnityEngine.Random.Range(0.5f, 3f);
            if (updateTime > 3f)
            {
                if (WaveManager.instance.GetcalibrationWave() == 1)
                {
                    easyWaveCalibration.Add(stress_15b);
                }
                else if (WaveManager.instance.GetcalibrationWave() == 2)
                {
                    mediumWaveCalibration.Add(stress_15b);
                }
                else if (WaveManager.instance.GetcalibrationWave() == 3)
                {
                    hardWaveCalibration.Add(stress_15b);
                }
                foreach (float value in easyWaveCalibration)
                {
                    Debug.Log("easyWaveCalibration : " + value);
                }
                foreach (float value in mediumWaveCalibration)
                {
                    Debug.Log("mediumWaveCalibration : " + value);
                }
                foreach (float value in hardWaveCalibration)
                {
                    Debug.Log("hardWaveCalibration : " + value);
                }
                updateTime = 0f;

            }

            else if (WaveManager.instance.isCalibrated)
            {

                // Debug.Log("stress_100b : " + stress_100b);
                // Debug.Log("stress_15b : " + stress_15b);
                // Debug.Log("hr : " + hr);
                // Debug.Log("Current FPS: " + (1.0f / Time.deltaTime));

                float stress_diff_15b = Math.Max(stress_15b - stress_ref, 0f);
                float stress_diff_100b = Math.Max(stress_100b - stress_ref, 0f);

                if (stress_100b > 0)
                {
                    stress_factor = Math.Min((0.7f * stress_diff_15b + 0.3f * stress_diff_100b) / (2f * stress_ref), 1f) / ((1.0f / Time.deltaTime) * 2.5f);
                }
                else
                {
                    stress_factor = 0f; // Évite NaN en cas de stress_100b = 0
                }

                Debug.Log("stress_diff_15b : " + stress_diff_15b);
                Debug.Log("stress_diff_100b : " + stress_diff_100b);
                Debug.Log("stress_factor : " + stress_factor);

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
            }
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
        heartRateBar.fillAmount = currentFillAmount;
        heartRateBar.color = Color.white;
        stressReachedText.gameObject.SetActive(false);
        hardWave = false;
    }



}
