using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DayNightController : MonoBehaviour
{

    [Header("Skybox")]
    public Material skyboxMaterial; 
    public AnimationCurve skyboxTintCurve;

    [Header("Cycle durations")]
    public float timeSpeed = 2000f; //speed of time
    public float start = 12f; //starting time
    public float sunrise;  //sunrise time
    public float sunset; //sunset time

    [Header("Light")]
    public Light sun;
    public Light moon;

    [Header("Colour")]
    public Color dayAmbience = new Color(0.627f, 0.765f, 0.537f);
    public Color nightAmbience = new Color(0.604f, 0.565f, 0.737f);

    [Header("Intensity")]
    public AnimationCurve lightCurve; //light transition curve
    public float maxSun = 1f; //max sun intentisty
    public float maxMoon = 0.8f;

    [Header("Fog")]
    public float normalFogDensity = 0.002f;
    public float thickerFogDensity = 0.01f;
    public AnimationCurve fogTransitionCurve;

    private DateTime currentTime; //track times
    private TimeSpan sunriseTime; 
    private TimeSpan sunsetTime;
    private bool isDay;

    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(start);
        sunriseTime = TimeSpan.FromHours(sunrise);
        sunsetTime = TimeSpan.FromHours(sunset);
        RenderSettings.fog = true;
        RenderSettings.fogDensity = normalFogDensity;
        sunrise = MainMenu.ChosenSunrise;
        sunset = MainMenu.ChosenSunset;
    }

    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        RotateMoon();
        AdjustLight();
        AdjustFog();
    }

    void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeSpeed);
    }
    public static DayNightController Instance { get; private set; }
    public bool IsDay { get { return currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime; } }
    private void Awake()
    {
        //only one instance of script
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; //singleton instance
            DontDestroyOnLoad(gameObject); //keep across scenes
        }
    }


    void RotateSun()
    {
        float sunRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            isDay = true;
            TimeSpan dayDuration = sunsetTime - sunriseTime;
            TimeSpan elapsedTime = currentTime.TimeOfDay - sunriseTime;

            float timeProgress = (float)(elapsedTime.TotalMinutes / dayDuration.TotalMinutes);
            sunRotation = Mathf.Lerp(0f, 180f, timeProgress);
        }
        else
        {
            isDay = false;
            TimeSpan nightDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan elapsedTime = currentTime.TimeOfDay - sunsetTime;

            float timeProgress = (float)(elapsedTime.TotalMinutes / nightDuration.TotalMinutes);
            sunRotation = Mathf.Lerp(180f, 360f, timeProgress);
        }

        sun.transform.rotation = Quaternion.AngleAxis(sunRotation, Vector3.right);
    }
    void RotateMoon()
    {
        float moonRotation = 0f;

        if (currentTime.TimeOfDay >= sunsetTime || currentTime.TimeOfDay <= sunriseTime)
        {
            TimeSpan nightDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan elapsedTime = currentTime.TimeOfDay - sunsetTime;

            if (elapsedTime.TotalSeconds < 0)
            {
                elapsedTime += TimeSpan.FromHours(24);//adjust when crosses midnight
            }

            float timeProgress = (float)(elapsedTime.TotalMinutes / nightDuration.TotalMinutes);
            moonRotation = Mathf.Lerp(0f, 180f, timeProgress);
        }

        moon.transform.rotation = Quaternion.AngleAxis(moonRotation, Vector3.right);
    }

    void AdjustLight()
    {
        float sunlightDotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
        float lightFactor = lightCurve.Evaluate(sunlightDotProduct);
        sun.intensity = Mathf.Lerp(0f, maxSun, lightCurve.Evaluate(sunlightDotProduct));
        moon.intensity = Mathf.Lerp(maxMoon, maxSun, lightCurve.Evaluate(sunlightDotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbience, dayAmbience, lightCurve.Evaluate(sunlightDotProduct));

        if (skyboxMaterial != null)
        {
            //exposure range for day (0.8) and night (2.0)
            float exposure = Mathf.Lerp(0.5f, 0.9f, lightFactor);
            skyboxMaterial.SetFloat("_Exposure", exposure);
        }
    }

    void AdjustFog()
    {
        float timeProgress = 0f;
        //night
        if (currentTime.TimeOfDay > sunsetTime || currentTime.TimeOfDay < sunriseTime)
        {
            //increase fog gradually
            TimeSpan nightDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan elapsedTime = currentTime.TimeOfDay - sunsetTime;

            //wrap around midnight
            if (elapsedTime.TotalSeconds < 0)
            {
                elapsedTime += TimeSpan.FromHours(24);
            }

            //calc from sunset to sunrise
            timeProgress = (float)(elapsedTime.TotalMinutes / nightDuration.TotalMinutes);
            RenderSettings.fogDensity = Mathf.Lerp(normalFogDensity, thickerFogDensity, fogTransitionCurve.Evaluate(timeProgress));
        }
        else if (currentTime.TimeOfDay >= sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            //before sun, decrease
            TimeSpan dayDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan elapsedTime = currentTime.TimeOfDay - sunriseTime;

            timeProgress = (float)(elapsedTime.TotalMinutes / dayDuration.TotalMinutes);
            RenderSettings.fogDensity = Mathf.Lerp(thickerFogDensity, normalFogDensity, fogTransitionCurve.Evaluate(timeProgress));
        }
    }

    TimeSpan CalculateTimeDifference(TimeSpan startTime, TimeSpan endTime)
    {
        TimeSpan difference = endTime - startTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}