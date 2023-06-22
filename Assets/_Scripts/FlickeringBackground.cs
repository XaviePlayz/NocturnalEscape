using System.Collections;
using UnityEngine;

public class FlickeringBackground : MonoBehaviour
{
    public GameObject darkBackground;
    public GameObject flickeringObject;
    public float flickerIntervalMin = 2f;
    public float flickerIntervalMax = 5f;
    public float flickerDuration = 0.1f;
    public float flickerDelay = 1f;
    public int flickerCount = 3;
    public float darkDuration = 3f;
    public int objectAppearanceChance = 3;
    public bool hasAppeared;
    public int hasNotAppearedAfterTwoFlickers = 0;

    private bool isFlickering = false;

    public AudioVolumeController audioController;

    private void Start()
    {
        audioController.GetComponent<AudioVolumeController>();
        Invoke("StartFlickering", Random.Range(flickerIntervalMin, flickerIntervalMax));
    }

    private void StartFlickering()
    {
        if (!isFlickering)
        {
            isFlickering = true;
            if (audioController.options.activeInHierarchy)
            {
                StartCoroutine(FlickerCoroutine());

            }
            else
            {
                StopCoroutine(FlickerCoroutine());
            }
        }
    }

    private IEnumerator FlickerCoroutine()
    {
        while (true)
        {
            // Rapid flickering
            for (int i = 0; i < flickerCount; i++)
            {
                darkBackground.SetActive(!darkBackground.activeSelf);
                yield return new WaitForSeconds(flickerDuration);
            }

            // Transition to full light state
            darkBackground.SetActive(false);

            // Delay before the next flickering
            yield return new WaitForSeconds(flickerDelay);

            // Rapid flickering again
            for (int i = 0; i < flickerCount; i++)
            {
                darkBackground.SetActive(!darkBackground.activeSelf);
                yield return new WaitForSeconds(flickerDuration);
            }

            // Transition to dark state
            flickeringObject.SetActive(false);
            darkBackground.SetActive(true);

            int monsterChance = Random.Range(0, objectAppearanceChance);
            // Check if the object should appear between flickers and the darkBackground is true
            Debug.Log(monsterChance);
            if (monsterChance == 1 && !hasAppeared || hasNotAppearedAfterTwoFlickers >= 2)
            {
                flickeringObject.SetActive(true);
                hasAppeared = true;
                hasNotAppearedAfterTwoFlickers = 0;
            }
            else
            {
                flickeringObject.SetActive(false);
                hasAppeared = false;
                hasNotAppearedAfterTwoFlickers++;
            }

            // Delay before the next cycle
            yield return new WaitForSeconds(darkDuration);
        }
    }
}