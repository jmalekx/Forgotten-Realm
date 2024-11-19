using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;  // Assign the directional light as the "sun"
    public Light moon; // Assign a directional light as the "moon"
    public float dayLength = 120f;  // Full day duration in seconds
    public Gradient lightColor;  // Controls the sun's color over time
    public Gradient moonColor;  // Controls the moon's color over time
    public AnimationCurve moonIntensityCurve; // Optional: Curve to control moonlight intensity

    private float time = 0f;  // Keeps track of the current time of day

    void Update()
    {
        // Increment time based on real-time progression
        time += Time.deltaTime;

        // Normalize time value (0 = start of day, 1 = end of day)
        float dayProgress = time / dayLength;

        // Reset the time to start a new day after a full cycle
        if (dayProgress >= 1f)
        {
            time = 0f;
            dayProgress = 0f;
        }

        // Calculate sun angle (0 to 180 degrees from mid-morning to sunset)
        float sunAngle = Mathf.Lerp(0f, 180f, dayProgress);
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Change sun's color and intensity over the day
        sun.color = lightColor.Evaluate(dayProgress);
        sun.intensity = Mathf.Clamp01(Mathf.Cos(dayProgress * Mathf.PI));

        // Moon setup
        if (moon != null)
        {
            // Calculate moon angle (opposite to the sun's path)
            float moonAngle = Mathf.Lerp(180f, 360f, dayProgress);
            moon.transform.rotation = Quaternion.Euler(moonAngle, -170f, 0f);

            // Adjust moon's color and intensity dynamically
            moon.color = moonColor.Evaluate(dayProgress);
            moon.intensity = moonIntensityCurve.Evaluate(dayProgress);

            // Enable moon only at night
            moon.enabled = dayProgress > 0.25f && dayProgress < 0.75f;
        }

        // Optional: Adjust ambient light
        RenderSettings.ambientLight = Color.Lerp(
            new Color(0.02f, 0.02f, 0.1f), // Dark blue for night
            Color.white,                  // Bright white for day
            Mathf.Clamp01(sun.intensity)
        );
    }
}
