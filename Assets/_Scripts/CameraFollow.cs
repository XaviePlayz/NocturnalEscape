using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector3 offset; // Offset from the player's position
    public float maxLeftOffset = 5f; // Maximum offset to the left from the player
    public float maxRightOffset = 5f; // Maximum offset to the right from the player

    private float minPosX; // Minimum X position of the camera
    private float maxPosX; // Maximum X position of the camera
    private float minPosY; // Minimum Y position of the camera
    private float maxPosY; // Maximum Y position of the camera

    private void Start()
    {
        // Calculate the minimum and maximum positions based on the map's boundaries
        minPosX = offset.x - maxLeftOffset;
        maxPosX = offset.x + maxRightOffset;
        minPosY = offset.y;
    }

    private void LateUpdate()
    {
        // Calculate the desired camera position
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosX, maxPosX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosY, maxPosY);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}