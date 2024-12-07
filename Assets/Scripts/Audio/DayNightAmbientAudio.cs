using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightAmbientAudio : MonoBehaviour
{
    [Header("Day/Night Ambient Audio")]
    public AudioClip dayAudioClip;
    public AudioClip nightAudioClip;
    public float fadeDuration = 2f;

    private AudioSource audioSource;
    private bool isDay = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        //play day ambient
        audioSource.clip = dayAudioClip;
        audioSource.Play();
    }

    void Update()
    {
        //check if day/night then adjust
        bool currentIsDay = DayNightController.Instance.IsDay;

        if (currentIsDay != isDay)
        {
            isDay = currentIsDay;
            StartCoroutine(FadeToNewClip(isDay ? dayAudioClip : nightAudioClip));
        }
    }

    IEnumerator FadeToNewClip(AudioClip newClip)
    {
        //fade audio
        float elapsedTime = 0f;
        float originalVolume = audioSource.volume;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(originalVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();

        //change clip and fade
        audioSource.clip = newClip;
        audioSource.Play();

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, originalVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = originalVolume;
    }
}
