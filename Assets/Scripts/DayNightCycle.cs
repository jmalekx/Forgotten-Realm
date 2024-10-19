using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;  // Assign the directional light as the "sun"
    public Light moon; // Optional: Create a second light for the moon
    public float dayLength = 120f;  // Full day duration in seconds
    public Gradient lightColor;  // Controls the sun's color as it changes over time

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

        // Start the sun higher in the sky, at 0 degrees instead of -90
        float sunAngle = Mathf.Lerp(0f, 180f, dayProgress);  // 0 (mid-morning) to 180 (sunset)
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Change light color over the day
        sun.color = lightColor.Evaluate(dayProgress);

        // Adjust lighting intensity (starts bright and gets darker)
        sun.intensity = Mathf.Clamp01(Mathf.Cos(dayProgress * Mathf.PI));  // Starts bright, gets darker

        // Moon setup (optional): Enable moonlight at night
        if (moon != null)
        {
            if (dayProgress > 0.5f)
            {
                moon.enabled = true;
                moon.intensity = Mathf.Clamp01(Mathf.Cos((dayProgress - 0.5f) * Mathf.PI * 2));
            }
            else
            {
                moon.enabled = false;
            }
        }
    }
}
