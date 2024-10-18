using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed;
    public float moveDistance; //max distance to move per move
    public LayerMask ground;
    public float objectHeight;
    public float minRange;
    public float maxRange;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float stopDuration;
    private float stopTimer;

    void Start()
    {
        StopAndWait();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                ChooseNewTargetPosition();
            }
        }
    }

    void ChooseNewTargetPosition()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-moveDistance, moveDistance),
            0,
            Random.Range(-moveDistance, moveDistance)
        );

        targetPosition = transform.position + randomDirection;

        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            targetPosition = hit.point + Vector3.up * objectHeight; //ensure the object stays above the terrain
            isMoving = true;
        }
        else
        {
            //if invalid target, choose again
            ChooseNewTargetPosition();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) //check if reached target
        {
            isMoving = false;
            StopAndWait();
        }
    }

    void StopAndWait()
    {
        stopDuration = Random.Range(minRange, maxRange);
        stopTimer = stopDuration;
        isMoving = false;
    }
}
