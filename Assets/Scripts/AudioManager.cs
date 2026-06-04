using UnityEngine;
using TMPro;

// script to manage game audio and toggle music on/off
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool audioOn = true; // tracks if audio is enabled
    private AudioSource audioSource;

    public TMP_Text audioButtonText; // UI text for button

    void Awake()
    {
        // singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        UpdateButtonText();
    }

    // toggle audio on/off
    public void ToggleAudio()
    {
        audioOn = !audioOn;

        // pause or play music
        if (audioOn)
        {
            audioSource.Play();
            AudioListener.pause = false;
        }
        else
        {
            audioSource.Pause();
            AudioListener.pause = true;
        }

        UpdateButtonText();
    }

    // update UI text
    void UpdateButtonText()
    {
        if (audioButtonText != null)
        {
            audioButtonText.text = audioOn ? "Audio ON" : "Audio OFF";
        }
    }
}