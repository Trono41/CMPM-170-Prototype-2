using UnityEngine;

public class BuildingRunAround : MonoBehaviour
{
    public float moveRadius = 10f;   // how far from the start they can roam
    public float moveSpeed = 6f;     // how fast they move
    public float pauseTime = 1f;     // how long to pause before picking a new point

    private Vector3 startPos;       
    private Vector3 targetPos;
    private float pauseTimer;

    void Start()
    {
        startPos = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            pauseTimer = pauseTime;
            PickNewTarget();
        }
    }

    private void PickNewTarget()
    {
        Vector2 rand = Random.insideUnitCircle * moveRadius;
        targetPos = startPos + new Vector3(rand.x, 0f, rand.y);
    }
}