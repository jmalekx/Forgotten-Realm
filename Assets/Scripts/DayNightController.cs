using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;  // Assign the directional light as the "sun"
    public Light moon; // Assign a directional light as the "moon"
    public float dayLength = 120f;  // Full day duration in seconds
    public Gradient lightColor;  // Controls the sun's color over time
    public Gradient moonColor;  // Optional: Controls the moon's color over time

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

            // Enable moon only at night
            if (dayProgress > 0.5f || dayProgress < 0.1f)  // Moon is on during dusk, night, and dawn
            {
                moon.enabled = true;
                moon.intensity = 0.5f;
                moon.color = Color.white; // Temporarily set a bright color to test

            }
            else
            {
                moon.enabled = false;
            }
        }
    }
}
