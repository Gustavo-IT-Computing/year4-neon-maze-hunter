using UnityEngine;
using TMPro;

// script to manage the countdown timer and trigger game over when time runs out
public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f; // total time for the level
    public TMP_Text timerText; // UI text to display remaining time
    public GameObject gameOverUI; // UI shown when time is up
    private bool gameEnded = false; // prevents timer from running after game ends
    private bool gameWon = false;

    void Update()
    {
        // stop updating if game already ended
        if (gameEnded || gameWon) return;

        // decrease time every frame 
        timeRemaining -= Time.unscaledDeltaTime;

        // check if time has run out
        if (timeRemaining <= 0)
        {
            timeRemaining = 0; // prevent negative time
            EndGame(); // trigger game over
        }

        UpdateTimerUI(); // update UI every frame
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // display time rounded up to nearest whole number
            timerText.text = "Timer: " + Mathf.Ceil(timeRemaining);
        }
    }

    void EndGame()
    {
        gameEnded = true; // stop further updates

        // find GameManager and call GameOver function
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver(); // trigger game over logic
        }

        Time.timeScale = 0f; // pause the game
    }

    public void StopTimer()
    {
        gameWon = true;
    }
}