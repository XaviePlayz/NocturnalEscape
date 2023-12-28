using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public SpriteMask flashlightMask;
    public SpriteRenderer flashlightSpriteRenderer;
    public float flashlightRadius = 0.5f; // Adjust this value in the Inspector
    private bool isFlashlightOn = true;

    void Update()
    {
        // Check for player input to turn the flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightMask.enabled = isFlashlightOn;
            flashlightSpriteRenderer.enabled = isFlashlightOn;

            // Pass the flashlight radius to the shader
            flashlightMask.GetComponent<Renderer>().material.SetFloat("_Radius", isFlashlightOn ? flashlightRadius : 0f);
        }
    }
}
