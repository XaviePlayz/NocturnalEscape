using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NoiseMeterUI : MonoBehaviour
{
    public Slider meterSlider;
    public Gradient gradient;

    private float currentNoiseLevel;
    private float previousNoiseLevel;
    private Coroutine meterCoroutine;
    private float maxNoiseLevel;

    public void Initialize(float maxLevel)
    {
        maxNoiseLevel = maxLevel;
    }

    public float GetMaxNoiseLevel()
    {
        return maxNoiseLevel;
    }

    public void SetNoiseLevel(float noiseLevel)
    {
        previousNoiseLevel = currentNoiseLevel;
        currentNoiseLevel = noiseLevel;

        if (meterCoroutine != null)
        {
            StopCoroutine(meterCoroutine);
        }

        meterCoroutine = StartCoroutine(UpdateMeterFill());
    }

    public void IncreaseNoiseLevel(float noiseIncreaseRate)
    {
        currentNoiseLevel += noiseIncreaseRate;
        currentNoiseLevel = Mathf.Clamp(currentNoiseLevel, 0f, maxNoiseLevel);
        SetNoiseLevel(currentNoiseLevel);
    }

    public void DecreaseNoiseLevel(float noiseDecreaseRate)
    {
        currentNoiseLevel -= noiseDecreaseRate;
        currentNoiseLevel = Mathf.Clamp(currentNoiseLevel, 0f, maxNoiseLevel);
        SetNoiseLevel(currentNoiseLevel);
    }

    private IEnumerator UpdateMeterFill()
    {
        float elapsedTime = 0f;
        float duration = 0.3f; // Change this value to adjust the duration of the transition

        float startValue = meterSlider.value;
        float endValue = currentNoiseLevel / maxNoiseLevel;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float value = Mathf.Lerp(startValue, endValue, t);

            meterSlider.value = value;

            Color color = gradient.Evaluate(value);
            meterSlider.fillRect.GetComponent<Image>().color = color;

            yield return null;
        }

        meterCoroutine = null;
    }

    public void ResetNoiseLevel()
    {
        currentNoiseLevel = previousNoiseLevel;

        if (meterCoroutine != null)
        {
            StopCoroutine(meterCoroutine);
        }

        meterCoroutine = StartCoroutine(UpdateMeterFill());
    }
}