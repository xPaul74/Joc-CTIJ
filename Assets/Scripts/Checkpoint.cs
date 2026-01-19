using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointReached = false;
    public float raiseAmount = 1f;

    public AudioSource audioSource;
    public AudioClip checkpointSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDeath player = other.GetComponent<PlayerDeath>();
        if (player != null && !checkpointReached)
        {
            player.SetRespawnPoint(transform.position);
            checkpointReached = true;
            transform.position += new Vector3(0, raiseAmount, 0);
            audioSource.PlayOneShot(checkpointSound);
            Debug.Log("Checkpoint atins!");
        }
    }
}
