using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public float distance = 3f;

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float currentDistance = Vector2.Distance(startPosition, transform.position);

        if (currentDistance >= distance)
        {
            Flip();
        }

        float direction = movingRight ? 1f : -1f;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void Flip()
    {
        movingRight = !movingRight;
        startPosition = transform.position;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}