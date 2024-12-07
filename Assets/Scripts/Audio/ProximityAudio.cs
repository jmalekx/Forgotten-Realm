using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProximityAudio : MonoBehaviour
{
    [System.Serializable]
    public class AudioArea
    {
        public Transform areaTransform;  //area of audio
        public AudioClip soundClip;
        public float maxDistance = 10f;
        public float minVolume = 0.1f;  
        public float maxVolume = 1f; 
    }

    public AudioArea[] audioAreas;  //array of areas and audios
    private AudioSource audioSource;
    private AudioArea currentAudioArea;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        //closest audio area
        AudioArea closestArea = null;
        float closestDistance = Mathf.Infinity;

        foreach (var area in audioAreas)
        {
            //distance form player
            float distance = Vector3.Distance(transform.position, area.areaTransform.position);

            //closest area
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestArea = area;
            }
        }

        if (closestArea != null)
        {
            //adjust audio based on area proximity
            float volume = Mathf.Lerp(closestArea.maxVolume, closestArea.minVolume, closestDistance / closestArea.maxDistance);
            audioSource.volume = volume;

            //only play if within range
            if (closestDistance < closestArea.maxDistance && audioSource.clip != closestArea.soundClip)
            {
                audioSource.clip = closestArea.soundClip;
                audioSource.Play();
            }
            else if (closestDistance >= closestArea.maxDistance && audioSource.isPlaying)
            {
                audioSource.Stop();  //stop when far
            }
        }
    }
}

