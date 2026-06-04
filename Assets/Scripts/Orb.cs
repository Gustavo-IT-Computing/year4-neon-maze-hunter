using UnityEngine;

// script for collectible orbs that increase score and track progress
public class Orb : MonoBehaviour
{
    private ScoreManager scoreManager; // reference to score system
    private GameManager gameManager; // reference to game manager
    private AudioSource audioSource; // audio source for sound effects
    public AudioClip orbSound; // sound played when orb is collected

    void Start()
    {
        // find ScoreManager and GameManager in the scene
        scoreManager = FindFirstObjectByType<ScoreManager>();
        gameManager = FindFirstObjectByType<GameManager>();

        // find AudioManager object
        GameObject audioManager = GameObject.Find("AudioManager");

        if (audioManager != null)
        {
            // get audio source from AudioManager
            audioSource = audioManager.GetComponent<AudioSource>();
        }

    }

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only react if player touches the orb
        if (!other.CompareTag("Player")) return;

        // prevent multiple triggers
        if (collected) return;
        collected = true;

        // disable collider immediately (VERY IMPORTANT)
        GetComponent<Collider2D>().enabled = false;

        // add 1 point to score
        if (scoreManager != null)
            scoreManager.AddScore(1);

        // notify GameManager that an orb was collected
        if (gameManager != null)
            gameManager.OrbCollected();

        // play collection sound
        if (AudioManager.instance != null && AudioManager.instance.audioOn)
        {
            audioSource.PlayOneShot(orbSound);
        }

        // destroy orb safely
        Destroy(gameObject);
    }
}