using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string playerName;
    public TMP_Text displayNameText;

    public void SaveName()
    {
        if (nameInput == null)
        {
            Debug.LogError("NameInput is NOT assigned!");
            return;
        }

        playerName = nameInput.text;

        if (displayNameText != null)
            displayNameText.text = "Player: " + playerName;
    }
}