using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float batteryLife = 100f; // Drone starts with full battery
    public float speed = 5f;          // Speed of the drone
    public float rotationSpeed = 50f; // Rotation speed of the drone
    public bool isRedDrone = false;   // This will help us differentiate red and blue drones
    private Vector3 movementDirection; // Movement direction for the drone

    // Initialize the drone with a specific color
    public void SetInitialColor(bool red)
    {
        Renderer droneRenderer = GetComponent<Renderer>();
        isRedDrone = red;

        if (isRedDrone)
        {
            droneRenderer.material.color = Color.red;  // Red color
        }
        else
        {
            droneRenderer.material.color = Color.blue; // Blue color
        }
    }

    // Method to set the drone's movement direction
    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    void Update()
    {
        if (batteryLife > 0)
        {
            // Move the drone in the specified direction
            transform.Translate(movementDirection * speed * Time.deltaTime);
            
            // Optional: add battery depletion over time
            batteryLife -= Time.deltaTime; 
        }
        else
        {
            Debug.Log("Battery depleted. Drone has stopped.");
        }
    }
}




