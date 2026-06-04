using System.Collections;
using UnityEngine;
using TMPro;

// script to handle player power up state, effects, and duration
public class PlayerPower : MonoBehaviour
{
    public bool isPowered = false; // tracks if player is powered
    public AudioClip powerSound; // sound played during power up
    private AudioSource audioSource; // audio component
    private Animator animator; // controls player animations
    public TMP_Text powerText; // UI text to show power status

    void Start()
    {
        // get components from player
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void ActivatePower(float duration)
    {
        // start power up coroutine with given duration
        StartCoroutine(PowerRoutine(duration));
    }

    IEnumerator PowerRoutine(float duration)
    {
        // activate power
        isPowered = true;

        // update UI
        if (powerText != null)
            powerText.text = "Power: ON";

        // trigger powered animation
        if (animator != null)
            animator.SetBool("isPowered", true);

        // play looping power sound
        if (AudioManager.instance != null && AudioManager.instance.audioOn)
        {
            audioSource.clip = powerSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        // wait for duration of power up
        yield return new WaitForSeconds(duration);

        // deactivate power
        isPowered = false;

        // update UI
        if (powerText != null)
            powerText.text = "Power: OFF";

        // return to normal animation
        if (animator != null)
            animator.SetBool("isPowered", false);

        // stop power sound
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }
}