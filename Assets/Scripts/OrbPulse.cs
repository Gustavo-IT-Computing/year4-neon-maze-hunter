using UnityEngine;

// script to create a pulsing (breathing) animation effect for orbs
public class OrbPulse : MonoBehaviour
{
    public float speed = 2f; // controls how fast the orb pulses
    public float amount = 0.05f; // controls how strong the scale change is

    private Vector3 startScale; // stores the original scale of the orb

    void Start()
    {
        // save the initial scale so we can pulse relative to it
        startScale = transform.localScale;
    }

    void Update()
    {
        // calculate a smooth oscillation using sine wave
        // this creates a continuous "grow and shrink" effect
        float scaleFactor = 1 + Mathf.Sin(Time.time * speed) * amount;

        // apply the scale while keeping the original proportions
        transform.localScale = startScale * scaleFactor;
    }
}