using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Animator animator;
    public string idleAnimationTrigger = "Idle";
    public string walkAnimationTrigger = "Walk";
    public string jumpAnimationTrigger = "Jump";
    public string fallAnimationTrigger = "Fall";
    public string hideAnimationTrigger = "Hide";
    public string hurtAnimationTrigger = "Hurt";

    public Transform respawnPoint;

    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isFacingRight = true;
    private bool isRespawning = false;
    public bool isHiding = false;
    public bool isPlayerVisible = true;

    [SerializeField] private InteractableObject currentInteractable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isRespawning)
        {
            if (!isHiding)
            {
                float moveX = Input.GetAxis("Horizontal");

                // Move the player horizontally
                rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

                if (moveX > 0 && !isFacingRight)
                {
                    // Player is moving right and facing left, flip the character
                    FlipCharacter();
                }
                else if (moveX < 0 && isFacingRight)
                {
                    // Player is moving left and facing right, flip the character
                    FlipCharacter();
                }

                if (moveX != 0)
                {
                    // Player is walking
                    animator.SetTrigger(walkAnimationTrigger);
                }

                else if (!isJumping && !isHiding)
                {
                    // Player is idle and on the ground, trigger the Idle animation
                    animator.SetTrigger(idleAnimationTrigger);
                }

                if (Input.GetButtonDown("Jump") && !isJumping && isPlayerVisible)
                {
                    // Jump when the Jump button is pressed
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                    isJumping = true;
                    animator.SetTrigger(jumpAnimationTrigger);
                }

                //Start Crouching (Hiding)
                if (Input.GetKeyDown(KeyCode.S) && !isJumping && moveX == 0)
                {
                    // Hide animation triggered when the S key is pressed and the player is on the ground and standing still
                    animator.SetBool(hideAnimationTrigger, true);
                    isHiding = true;
                }
            }

            //Stop Crouching (Hiding)
            if (Input.GetKeyUp(KeyCode.S))
            {
                isHiding = false;
                animator.SetBool(hideAnimationTrigger, false);
                animator.SetTrigger(idleAnimationTrigger);
            }

            //Interact
            if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null && !isJumping)
            {
                isPlayerVisible = false;
                currentInteractable.Interact();
            }

            if (rb.velocity.y < 0)
            {
                // Player is falling
                animator.SetTrigger(fallAnimationTrigger);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentInteractable = other.GetComponent<InteractableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && isPlayerVisible)
        {
            currentInteractable = null;         
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Reset jumping state when touching the ground
            isJumping = false;
        }
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage()
    {
        if (!isRespawning)
        {
            // Player takes damage (e.g., enemy collision)
            isRespawning = true;
            animator.SetTrigger(hurtAnimationTrigger);
            rb.velocity = Vector2.zero;
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        // Freeze player movement
        rb.velocity = Vector2.zero;
        isRespawning = true;

        // Wait for the duration of the hurt animation
        yield return new WaitForSeconds(GetAnimationDuration(animator, hurtAnimationTrigger));

        // Respawn player
        transform.position = respawnPoint.position;
        //animator.SetTrigger(idleAnimationTrigger);
        isRespawning = false;
    }

    private float GetAnimationDuration(Animator animator, string animationTrigger)
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimationClip clip = clipInfo[0].clip;
        return clip.length;
    }
}
