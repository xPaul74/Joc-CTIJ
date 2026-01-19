using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerDeath playerDeath; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerDeath = Object.FindFirstObjectByType<PlayerDeath>();

        if (playerDeath == null)
        {
            Debug.LogError("GameManager: No PlayerDeath found");
        }
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("GameManager: Player died. Respawning");

        Vector3 respawnPosition = playerDeath.GetRespawnPoint();
        playerDeath.transform.position = respawnPosition;

        playerDeath.gameObject.SetActive(true);

        Rigidbody2D rb = playerDeath.GetComponent<Rigidbody2D>();

        PlayerMovement pm = playerDeath.GetComponent<PlayerMovement>();


        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 2.5f;
        }

        if (pm != null)
        {
            pm.StopAllCoroutines();
            pm.ResetMovementOnRespawn();
        }

    }
}
