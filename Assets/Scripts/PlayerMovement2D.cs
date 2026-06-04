using UnityEngine;

// script to control player movement 
public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 10f; // movement speed of the player
    public float acceleration = 20f;   // faster response (not currently used)
    public float deceleration = 25f;   // faster stop (not currently used)

    private Rigidbody2D rb; // reference to the Rigidbody2D component
    private Vector2 moveInput; // stores player input for movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // get Rigidbody2D component attached to the player

        rb.gravityScale = 0; // disable gravity for top-down movement
        rb.freezeRotation = true; // prevent player from rotating on collisions
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // improve collision accuracy
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // smooth movement between physics updates

        rb.linearDamping = 5f; // helps stop the player faster
    }

    void Update()
    {
        // get raw input from keyboard (WASD or arrow keys)
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized; // normalize to maintain consistent speed in all directions
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * speed; // calculate desired movement velocity

        // instant stop when no input
        if (moveInput.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector2.zero; // stop all movement
            return;
        }

        // instant direction change (no sliding)
        rb.linearVelocity = targetVelocity; // apply movement directly
    }
}