using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private bool isPlayerInvisible = false;
    private Renderer playerRenderer;

    public SpriteRenderer spriteRenderer;
    public Sprite glowSprite;
    public float glowIntensity = 0.5f;

    private Sprite originalSprite;
    private Color originalColor;
    private bool isInRange = false;

    public PlayerController playerController;

    [Header("Canvas")]
    public KeyCode interactionKey = KeyCode.E;
    public Text interactionText;

    private void Awake()
    {
        if (playerController != null)
        {
            playerRenderer = playerController.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("PlayerController not found!");
        }
        originalSprite = spriteRenderer.sprite;
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            SetGlowEffect(true);

            interactionText.gameObject.SetActive(true);
            interactionText.text = "Press " + interactionKey.ToString() + " to interact";
            interactionText.transform.position = transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            SetGlowEffect(false);

            interactionText.gameObject.SetActive(false);
        }
    }

    private void SetGlowEffect(bool enabled)
    {
        if (enabled)
        {
            spriteRenderer.sprite = glowSprite;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, glowIntensity);
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
            spriteRenderer.color = originalColor;
        }
    }

    public void Interact()
    {
        isPlayerInvisible = !isPlayerInvisible;
        playerRenderer.enabled = !isPlayerInvisible;

        if (isPlayerInvisible)
        {
            playerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            playerController.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            playerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            playerController.GetComponent<Collider2D>().enabled = true;
            playerController.isPlayerVisible = true;
        }
    }
}
