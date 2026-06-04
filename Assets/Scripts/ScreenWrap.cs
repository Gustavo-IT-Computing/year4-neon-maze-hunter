using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    public float leftBound = -6.7f;
    public float rightBound = 6.7f;

    public float tunnelYMin = -0.5f;
    public float tunnelYMax = 0.5f;

    public float offset = 0.3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;

        if (pos.y > tunnelYMin && pos.y < tunnelYMax)
        {
            if (pos.x >= rightBound - offset)
            {
                pos.x = leftBound + offset;
                rb.position = pos;
            }
            else if (pos.x <= leftBound + offset)
            {
                pos.x = rightBound - offset;
                rb.position = pos;
            }
        }
    }
}