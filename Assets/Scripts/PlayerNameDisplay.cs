using UnityEngine;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    public TMP_Text nameText;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        if (nameText != null)
            nameText.text = "Player: " + playerName;
    }
}

