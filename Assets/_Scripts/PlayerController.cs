using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Footstep Souns")]
    private AudioSource footstepsAudioSource;
    public AudioClip footstepsSound;
    private bool isPlayingFootsteps = false;

    [Header("Animator")]
    public Animator animator;
    public string idleAnimationTrigger = "Idle";
    public string walkAnimationTrigger = "Walk";
    public string jumpAnimationTrigger = "Jump";
    public string fallAnimationTrigger = "Fall";
    public string hideAnimationTrigger = "Hide";
    public string hurtAnimationTrigger = "Hurt";

    [Header("Noise Indicator")]
    public float noiseIncreaseAmount = 0.1f;
    public float noiseDecreaseRate = 0.05f;
    public float walkingNoiseLevel = 0.2f; // The maximum noise level when walking
    private bool isMakingNoise = false;
    private bool isWalking = false;
    private bool isAlarmActive = false;
    private AudioSource audioSource;
    private NoiseMeterUI noiseMeterUI;

    public Transform respawnPoint;

    [Header("Booleans")]
    private Rigidbody2D rb;
    public bool isJumping = false;
    private bool isFacingRight = true;
    private bool isRespawning = false;
    public bool isHiding = false;
    public bool isPlayerVisible = true;
    public bool isUsingDoor = false;

    [Header("Door Interaction")]
    public AudioSource doorAudioSource;
    public AudioClip doorOpenSound;

    [Header("Closet Interaction")]
    public AudioSource closetAudioSource;
    public AudioClip closetOpenSound;

    [Header("Scripts")]
    [SerializeField] private InteractableObject currentInteractable;

    private void Awake()
    {
        footstepsAudioSource = gameObject.AddComponent<AudioSource>();
        footstepsAudioSource.playOnAwake = false;

        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        noiseMeterUI = FindObjectOfType<NoiseMeterUI>();
        noiseMeterUI.Initialize(1f); // Set the maximum noise level to 1
    }

    private void Update()
    {
        isWalking = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (isWalking && !isJumping)
        {
            noiseMeterUI.SetNoiseLevel(Mathf.Min(walkingNoiseLevel, noiseMeterUI.GetMaxNoiseLevel()));
        }
        else
        {
            noiseMeterUI.DecreaseNoiseLevel(noiseDecreaseRate);
            isPlayingFootsteps = false;
        }

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

                if (moveX != 0 && !isJumping)
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
                if (!isUsingDoor)
                {
                    isPlayerVisible = false;
                }
                else
                {
                    // Play door open sound when interacting with the door
                    doorAudioSource.clip = doorOpenSound;
                    doorAudioSource.Play();
                }


                if(!isPlayerVisible)
                {
                    // Play door open sound when interacting with the door
                    closetAudioSource.clip = closetOpenSound;
                    closetAudioSource.Play();
                }
                currentInteractable.Interact();
            }

            if (rb.velocity.y < 0 && isJumping)
            {
                // Player is falling
                animator.SetTrigger(fallAnimationTrigger);
            }
        }
    }

    public void PlayFootstepsSound()
    {
        if (!isPlayingFootsteps)
        {
            // Set the footsteps sound clip
            footstepsAudioSource.clip = footstepsSound;
            // Play the sound
            footstepsAudioSource.Play();
            isPlayingFootsteps = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable_Door"))
        {
            currentInteractable = other.GetComponent<InteractableObject>();
            isUsingDoor = true;
        }
        else if (other.CompareTag("Interactable_Object"))
        {
            currentInteractable = other.GetComponent<InteractableObject>();
            isUsingDoor = false;
        }
        else if (other.CompareTag("Alarm"))
        {
            isAlarmActive = true;
            noiseMeterUI.SetNoiseLevel(noiseMeterUI.GetMaxNoiseLevel());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable_Door") && isPlayerVisible)
        {
            currentInteractable = null;
            isUsingDoor = false;
        }
        else if (other.CompareTag("Interactable_Object") && isPlayerVisible)
        {
            currentInteractable = null;
            isUsingDoor = false;
        }
        else if (other.CompareTag("Alarm"))
        {
            isAlarmActive = false;
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