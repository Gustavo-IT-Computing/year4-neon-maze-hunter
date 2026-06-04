using UnityEngine;

// script for the power up object that gives the player a temporary ability
public class PowerCore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // get the PlayerPower script from the player
            PlayerPower power = other.GetComponent<PlayerPower>();

            // if the player has the power script, activate power
            if (power != null)
            {
                power.ActivatePower(5f); // activate power for 5 seconds
            }

            // destroy this power up after being collected
            Destroy(gameObject);
        }
    }
}