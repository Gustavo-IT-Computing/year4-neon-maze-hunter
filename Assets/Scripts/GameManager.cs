using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// script to manage game state 
public class GameManager : MonoBehaviour
{
    public int totalOrbs; // total number of orbs in the level
    private int collectedOrbs = 0; // how many orbs the player has collected
    public int lives = 3; // player lives
    public TMP_Text livesText; // UI text to display lives
    public GameObject gameOverUI; // UI shown when player loses
    public GameObject winUI; // UI shown when player wins
    private GameObject player; // reference to player
    private Vector3 playerStartPos; // starting position of player
    private bool isGameOver = false; // tracks if game is over
    public AudioClip gameOverSound; // sound for game over
    public AudioClip hitSound; // sound when player is hit
    private AudioSource audioSource; // audio component

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerStartPos = player.transform.position;

        audioSource = GetComponent<AudioSource>();

        totalOrbs = GameObject.FindGameObjectsWithTag("Orb").Length;

        UpdateLivesUI();
    }

    public void OrbCollected()
    {
        collectedOrbs++; // increase collected orbs count

        if (collectedOrbs >= totalOrbs) // check if all orbs are collected
        {
            WinGame();
        }
    }

    void WinGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextScene); // go to next level
        }
        else
        {
            // stop timer so GameOver is not triggered after winning
            GameTimer timer = FindFirstObjectByType<GameTimer>();

            if (timer != null)
            {
                timer.StopTimer();
            }

            // Final Win screen
            if (winUI != null)
            {
                winUI.SetActive(true);
                winUI.transform.SetAsLastSibling();

                ScoreManager sm = FindFirstObjectByType<ScoreManager>();
                TMP_Text winText = winUI.GetComponentInChildren<TMP_Text>();

                if (sm != null && winText != null)
                {
                    winText.text =
                        "YOU WIN!\n\nFinal Score: " +
                        sm.GetScore() +
                        "\n\nPress R to Restart";
                }
            }

            // pause audio and game
            AudioListener.pause = true;
            Time.timeScale = 0f;
        }
    }

    // called when player is hit by enemy
    public void LoseLife()
    {
        lives--;

        // play hit sound
        if (AudioManager.instance != null && AudioManager.instance.audioOn)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // trigger hit animation on player
        if (player != null)
        {
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }

        UpdateLivesUI(); // update lives UI

        // check if player still has lives
        if (lives > 0)
        {
            RespawnPlayer();
        }
        else
        {
            GameOver();
        }
    }

    void RespawnPlayer()
    {
        if (player != null)
        {
            // move player back to starting position
            player.transform.position = playerStartPos;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // stop movement

                // reset physics to avoid bugs
                rb.simulated = false;
                rb.simulated = true;
            }
        }
    }
    public void GameOver()
    {
        if (isGameOver) return; // prevent double calls

        isGameOver = true;

        ScoreManager.ResetScore();

        // show UI first 
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            gameOverUI.transform.SetAsLastSibling();
        }

        // stop aull adui
        AudioListener.pause = true;

        // pause game
        Time.timeScale = 0f;
    }
    void UpdateLivesUI()
    {
        // update lives text if it exists
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    void Update()
    {
        // allow restart only after game over
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            // resume game time
            Time.timeScale = 1f;

            // resume audio
            AudioListener.pause = false;

            // restart from Level1
            SceneManager.LoadScene("Level1");
        }

    }

    void FinalWin()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
            winUI.transform.SetAsLastSibling();
        }

        // show final score
        ScoreManager sm = FindFirstObjectByType<ScoreManager>();

        if (sm != null)
        {
            Debug.Log("FINAL SCORE: " + sm.GetScore());
        }

        Time.timeScale = 0f;
    }
}