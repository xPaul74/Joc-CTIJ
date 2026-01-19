using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Vector3 currentRespawnPoint;


    void Start()
    {
        currentRespawnPoint = transform.position;
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        currentRespawnPoint = newRespawnPoint;
    }

    public Vector3 GetRespawnPoint()
    {
        return currentRespawnPoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Die();
        }

        if (other.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePlayerDeath();
        }
        else
        {
            Debug.LogError("GameManager Instance is null. Cannot handle player death.");
        }
    }
}
