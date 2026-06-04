using System.Collections;
using UnityEngine;

// script to control enemy movement, behavior, and interaction with the player

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Chaser,
        Patrol,
        Ambush
    }

    public EnemyType enemyType;
    public float speed = 3f; // movement spped of the enemy 
    public float decisionTime = 0.2f; // how often the enemy chooses a new direction 
    private Transform player; // reference to the player 
    private Rigidbody2D rb; // physicis component 
    private Vector2 currentDirection; // current move direction 
    private float decisionTimer; // timer to control direction changes  
    private Vector3 spawnPosition; // position where the enemy respawns 
    private SpriteRenderer sr; // used to change enemy color and blinking
    private float blinkTimer; // controls blinking effect 
    private Collider2D col; // collider to detect collisions 
    public GameObject explosionEffect;

    void Start()
    {
        // getting the required components 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // configure rigidbody with no gravity and no rotation
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // find player using the tag player 
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // find spawn point on the scene
        GameObject spawn = GameObject.Find("EnemySpawn");
        if (spawn != null)
            spawnPosition = spawn.transform.position;
        else
            Debug.LogError("EnemySpawn NOT FOUND!");

        ResetEnemy(); // initiliaze enemy state 
    }

    void OnEnable()
    {
        ResetEnemy(); // reset enemy when it becomes active 
    }

    void ResetEnemy()
    {
        transform.position = spawnPosition; // move enemy to spawn position 
        currentDirection = Vector2.right; // set default moveemnt direction 
        decisionTimer = 0f; // reset timer 

        if (col != null) // ensuring that the collider is enabled 
            col.enabled = true;
    }

    void Update()
    {
        if (player == null) return;

        PlayerPower power = player.GetComponent<PlayerPower>();

        // if the player is powered, make the enemy blink
        if (power != null && power.isPowered)
        {
            blinkTimer += Time.deltaTime * 10f; // increase blink speed 
            float alpha = Mathf.Abs(Mathf.Sin(blinkTimer)); // creating blinking effect 
            sr.color = new Color(1, 0, 0, alpha); // red color with changing transparency 
        }
        else
        {
            sr.color = Color.red; // normal red color when player is not powered 
        }

        // rotating enemy to face movement direction like drone
        if (currentDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        decisionTimer -= Time.fixedDeltaTime;

        if (decisionTimer <= 0)
        {
            ChooseDirection();
            decisionTimer = decisionTime;
        }

        rb.linearVelocity = currentDirection.normalized * speed;
    }

    void ChooseDirection()
    {
        PlayerPower power = player.GetComponent<PlayerPower>();

        Vector2[] directions = new Vector2[]
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
        };

        Vector2 bestDir = currentDirection;

        // if powered → enemies run away instead of chasing
        float bestScore = (power != null && power.isPowered) ? -Mathf.Infinity : Mathf.Infinity;

        bool foundPath = false;

        foreach (Vector2 dir in directions)
        {
            // avoid instantly going backwards unless stuck
            if (dir == -currentDirection) continue;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.8f);

            if (hit.collider != null && hit.collider.CompareTag("Wall"))
                continue;

            foundPath = true;

            float score = 0f;

            if (enemyType == EnemyType.Chaser)
            {
                float distance = Vector2.Distance((Vector2)transform.position + dir, player.position);
                score = distance;
            }

            else if (enemyType == EnemyType.Ambush)
            {
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                Vector2 predicted = (Vector2)player.position + playerRb.linearVelocity * 1.2f;

                float distance = Vector2.Distance((Vector2)transform.position + dir, predicted);
                score = distance;
            }

            else if (enemyType == EnemyType.Patrol)
            {
                // small randomness so it doesn't jitter every frame
                if (Random.value < 0.7f)
                    return;

                currentDirection = directions[Random.Range(0, directions.Length)];
                return;
            }

            if (power != null && power.isPowered)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDir = dir;
                }
            }
            else
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestDir = dir;
                }
            }
        }

        if (foundPath)
        {
            currentDirection = bestDir;
        }
        else
        {
            // fallback if completely stuck
            currentDirection = -currentDirection;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return; // only react if colliding with player 

        PlayerPower power = collision.gameObject.GetComponent<PlayerPower>();
        GameManager gm = FindFirstObjectByType<GameManager>();
        ScoreManager sm = FindFirstObjectByType<ScoreManager>();

        if (power != null && power.isPowered)
        {
            // disable collider to avoid multiple triggers
            col.enabled = false;

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (sm != null)
                sm.AddScore(50);

            StartCoroutine(RespawnEnemy());
        }
        else
        {
            // player loses a life 
            if (gm != null)
                gm.LoseLife();
        }
    }

    public void ResetEnemyManually()
    {
        StopAllCoroutines(); // stop all running coroutines 

        rb.linearVelocity = Vector2.zero; // stop movement 

        // reset position and direction 
        transform.position = spawnPosition;
        currentDirection = Vector2.right;

        if (col != null) // reset ccollider safely 
        {
            col.enabled = false;
            col.enabled = true;
        }
    }

    IEnumerator RespawnEnemy()
    {
        // stop movement
        rb.linearVelocity = Vector2.zero;

        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(6f);

        // reset position
        transform.position = spawnPosition;
        currentDirection = Vector2.right;

        // re-enable
        sr.enabled = true;
        col.enabled = true;
    }

    IEnumerator HitPlayer(GameManager gm)
    {
        // disable collide to prevent multiple hits
        col.enabled = false;

        yield return new WaitForSeconds(0.2f);

        // damage player 
        if (gm != null)
            gm.LoseLife();

        yield return new WaitForSeconds(0.5f);

        col.enabled = true; // enable the collider 
    }

    IEnumerator TemporaryDisable()
    {
        // disable collider and stop movement 
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        GetComponent<Collider2D>().enabled = true; // re-enable collider 
    }
}