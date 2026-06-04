using UnityEngine;
using TMPro;

// script to manage player score and high score display
public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // UI text element
    public static int score = 0; // current player score (shared across instances)
    private int highScore = 0; // highest score saved
    private string bestPlayer = "None"; // name of player with highest score

    void Start()
    {
        // load saved values
        highScore = PlayerPrefs.GetInt("HighScore", 0); // retrieve saved high score
        bestPlayer = PlayerPrefs.GetString("BestPlayer", "None"); // retrieve best player name

        UpdateUI(); // update UI at start
    }

    public void AddScore(int amount)
    {
        score += amount; // increase score by given amount

        // check if new high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // save new high score

            // save player name with score
            string currentPlayer = PlayerPrefs.GetString("PlayerName", "Player"); // get current player name
            bestPlayer = currentPlayer;
            PlayerPrefs.SetString("BestPlayer", bestPlayer); // save best player name
        }

        UpdateUI(); // refresh UI
    }

    void UpdateUI()
    {
        // update score display if UI element is assigned
        if (scoreText != null)
        {
            scoreText.text =
                "Score: " + score +
                "\n\n\nBest: " + highScore +
                "\n\nBest Player: " + bestPlayer; // display current and best scores
        }
    }

    public int GetScore()
    {
        return score; // return current score
    }

    public static void ResetScore()
    {
        score = 0; // reset score
    }
}