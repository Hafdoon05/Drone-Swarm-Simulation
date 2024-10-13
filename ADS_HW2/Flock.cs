using UnityEngine;

public class Flock : MonoBehaviour
{
    private float lastPartitionTime = 0f;
    public float partitionInterval = 10f; // Time interval to partition drones
    private DroneController[] allDrones;
    private Vector3 redDroneDirection = Vector3.forward; // Initial direction for red drones
    private Vector3 blueDroneDirection = Vector3.forward; // Initial direction for blue drones

    // Define the x and z boundaries for the drones
    public float xBoundaryMin = -60f;  // Minimum boundary on the x-axis
    public float xBoundaryMax = 60f;   // Maximum boundary on the x-axis
    public float zBoundaryMin = -100f; // Minimum boundary on the z-axis
    public float zBoundaryMax = 100f;  // Maximum boundary on the z-axis

    // Adjustable drone speed
    public float speed = 0.5f;  // Slower speed for smoother movement

    void Start()
    {
        // Initialize drones with their colors and positions
        allDrones = FindObjectsOfType<DroneController>();

        for (int i = 0; i < allDrones.Length; i++)
        {
            // Assign the first half as red, and the second half as blue
            if (i < allDrones.Length / 2)
            {
                allDrones[i].SetInitialColor(true);  // Red drones
            }
            else
            {
                allDrones[i].SetInitialColor(false); // Blue drones
            }

            // Set initial movement direction for all drones (straight forward)
            allDrones[i].SetMovementDirection(Vector3.forward); // Move forward initially
        }
    }

    void Update()
    {
        // Call the partition function every 10 seconds
        if (Time.time - lastPartitionTime >= partitionInterval)
        {
            lastPartitionTime = Time.time;
            ChangeDronesDirection(); // Split drones and change directions
        }

        // Move drones based on their group (red or blue)
        foreach (var drone in allDrones)
        {
            if (drone.isRedDrone)
            {
                // Move red drones and check for U-turn at the boundaries
                drone.transform.Translate(redDroneDirection * speed * Time.deltaTime);
                CheckAndHandleBoundary(drone, ref redDroneDirection);
            }
            else
            {
                // Move blue drones and check for U-turn at the boundaries
                drone.transform.Translate(blueDroneDirection * speed * Time.deltaTime);
                CheckAndHandleBoundary(drone, ref blueDroneDirection);
            }
        }
    }

    // Change the movement direction of the drones
    private void ChangeDronesDirection()
    {
        // Alternate the movement direction for red and blue drones:
        // - Red drones move forward/backward
        // - Blue drones move left/right
        redDroneDirection = (redDroneDirection == Vector3.forward) ? Vector3.back : Vector3.forward;
        blueDroneDirection = (blueDroneDirection == Vector3.right) ? Vector3.left : Vector3.right;

        Debug.Log("Red and blue drones changed directions.");
    }

    // Check if the drone has hit the x or z boundary and reverse direction if needed (U-turn)
    private void CheckAndHandleBoundary(DroneController drone, ref Vector3 direction)
    {
        // Get the current drone position
        Vector3 dronePosition = drone.transform.position;

        // If the drone is close to or exceeds the X boundary, reverse the X direction
        if (dronePosition.x >= xBoundaryMax && direction == Vector3.right)
        {
            // Reverse direction to move left
            direction = Vector3.left;
        }
        else if (dronePosition.x <= xBoundaryMin && direction == Vector3.left)
        {
            // Reverse direction to move right
            direction = Vector3.right;
        }

        // If the drone is close to or exceeds the Z boundary, reverse the Z direction
        if (dronePosition.z >= zBoundaryMax && direction == Vector3.forward)
        {
            // Reverse direction to move backward
            direction = Vector3.back;
        }
        else if (dronePosition.z <= zBoundaryMin && direction == Vector3.back)
        {
            // Reverse direction to move forward
            direction = Vector3.forward;
        }
    }
}









