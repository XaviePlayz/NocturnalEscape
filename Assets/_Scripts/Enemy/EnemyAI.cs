using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform leftPoint;
    public Transform rightPoint;
    public float visionRange = 5f;
    public float noiseDetectionRange = 10f;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private PlayerController playerController;
    public bool isPlayerDetected = false;
    public AlarmClock activatedAlarmClock;
    public bool isMovingToAlarm = false;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isPlayerDetected && !isMovingToAlarm)
        {
            MoveTowardsPlayer();
        }
        else if (isMovingToAlarm)
        {
            MoveTowardsAlarm();
        }
        else if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (transform.position.x > rightPoint.position.x)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (transform.position.x < leftPoint.position.x)
            {
                Flip();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerController != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerController.transform.position);

            if (distanceToPlayer > visionRange)
            {
                isPlayerDetected = false;
                return;
            }

            if (distanceToPlayer <= noiseDetectionRange)
            {
                Vector2 directionToPlayer = playerController.transform.position - transform.position;
                rb.velocity = new Vector2(directionToPlayer.normalized.x * moveSpeed, rb.velocity.y);

                if (directionToPlayer.x > 0 && !movingRight)
                {
                    Flip();
                }
                else if (directionToPlayer.x < 0 && movingRight)
                {
                    Flip();
                }
            }
        }
    }

    private void MoveTowardsAlarm()
    {
        if (activatedAlarmClock != null)
        {
            float distanceToAlarm = Vector2.Distance(transform.position, activatedAlarmClock.transform.position);

            if (distanceToAlarm <= 0.1f)
            {
                isMovingToAlarm = false;
                return;
            }

            Vector2 directionToAlarm = activatedAlarmClock.transform.position - transform.position;
            rb.velocity = new Vector2(directionToAlarm.normalized.x * moveSpeed, rb.velocity.y);

            if (directionToAlarm.x > 0 && !movingRight)
            {
                Flip();
            }
            else if (directionToAlarm.x < 0 && movingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activatedAlarmClock = null;
            isMovingToAlarm = false;
            isPlayerDetected = true;
        }
        else if (collision.gameObject.CompareTag("Alarm"))
        {
            StartCoroutine(CheckSound());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = false;
        }
    }

    IEnumerator CheckSound()
    {
        yield return new WaitForSeconds(3f);
        activatedAlarmClock = null;
        isMovingToAlarm = false;
        StopCoroutine(CheckSound());
    }
}
