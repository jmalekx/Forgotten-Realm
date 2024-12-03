using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    [Header("Pulse")]
    public float pulseStrength = 1f;//vertical pulse
    public float pulseSpeed = 1f;

    [Header("Drift")]
    public float driftSpeed = 0.5f;
    public float driftRadius = 3f;
    public float minDriftInterval = 2f;
    public float maxDriftInterval = 5f;

    [Header("Rotation")]
    public float rotationSpeed = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private float pulseTime;
    private float driftTimer;
    private float nextDriftTime;

    private float randomPulseOffset;
    private float randomDriftTimeOffset;

    void Start()
    {
        startPosition = transform.position;
        randomPulseOffset = Random.Range(0f, Mathf.PI * 2f);  //randomise pulse start
        randomDriftTimeOffset = Random.Range(0f, maxDriftInterval);  //randomise drift start

        ChooseNewDriftTarget();
        SetNextDriftTime();
    }

    void Update()
    {
        //vertical pusle, smooth oscillation
        float verticalOffset = Mathf.Sin((pulseTime + randomPulseOffset) * pulseSpeed) * pulseStrength;
        pulseTime += Time.deltaTime;

        //move towards target
        Vector3 targetPositionWithVerticalOffset = new Vector3(targetPosition.x, startPosition.y + verticalOffset, targetPosition.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPositionWithVerticalOffset, ref currentVelocity, 1f / driftSpeed);

        //drift timer
        driftTimer += Time.deltaTime;
        if (driftTimer >= nextDriftTime)
        {
            ChooseNewDriftTarget();
            SetNextDriftTime();
        }

        //rotate to remain upright
        Quaternion uprightRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, rotationSpeed * Time.deltaTime);
    }

    void ChooseNewDriftTarget()
    {
        //random point in radius
        Vector3 randomOffset = new Vector3(
            Random.Range(-driftRadius, driftRadius),
            0,
            Random.Range(-driftRadius, driftRadius)
        );
        targetPosition = startPosition + randomOffset;
    }

    void SetNextDriftTime()
    {
        //randomise interval
        driftTimer = 0f;
        nextDriftTime = Random.Range(minDriftInterval, maxDriftInterval) + randomDriftTimeOffset;
    }
}
