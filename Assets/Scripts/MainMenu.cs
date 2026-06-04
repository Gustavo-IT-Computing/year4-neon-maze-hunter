using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void StartGame()
    {
        if (nameInput != null && nameInput.text != "")
        {
            PlayerPrefs.SetString("PlayerName", nameInput.text);
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", "Player");
        }

        SceneManager.LoadScene("Level1");
    }
}