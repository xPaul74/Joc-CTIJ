using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeRestart = 10f;
    public GameObject winUI; 

    private bool hasFinished = false;

    public AudioSource audioSource;
    public AudioClip winSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") && !hasFinished)
        {
            hasFinished = true;
            WinGame(collision.gameObject);
        }
    }

    void WinGame(GameObject player)
    {
        Debug.Log("Nivel Terminat!");
        audioSource.PlayOneShot(winSound);
        var movement = player.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        Invoke("RestartLevel", delayBeforeRestart);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}