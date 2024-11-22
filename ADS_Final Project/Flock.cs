using UnityEngine;

public class Flock : MonoBehaviour
{
    private float lastPartitionTime = 0f;
    public float partitionInterval = 10f; // Fixed time interval to partition drones
    private DroneController[] allDrones;

    public float xBoundaryMin = -60f;
    public float xBoundaryMax = 60f;
    public float zBoundaryMin = -100f;
    public float zBoundaryMax = 100f;

    // Binary Tree Communication Networks (Existing)
    public DroneBTCommunication redDroneComm;
    public DroneBTCommunication blueDroneComm;

    // Graph Communication Networks (New)
    private DroneNetworkCommunication redDroneNetwork;
    private DroneNetworkCommunication blueDroneNetwork;

    void Start()
    {
        allDrones = FindObjectsOfType<DroneController>();

        // Assign IDs to drones
        for (int i = 0; i < allDrones.Length; i++)
        {
            allDrones[i].Id = i + 1; // Assign IDs starting from 1
        }

        if (allDrones.Length >= 6)
        {
            // Initialize first 3 drones as red and moving forward
            for (int i = 0; i < 3; i++)
            {
                allDrones[i].SetInitialColor(true);  // Red drones
                allDrones[i].SetMovementDirection(Vector3.forward);
            }

            // Initialize next 3 drones as blue and moving right
            for (int i = 3; i < 6; i++)
            {
                allDrones[i].SetInitialColor(false); // Blue drones
                allDrones[i].SetMovementDirection(Vector3.right);
            }

            // Initialize Binary Tree Communication Networks (Existing)
            DroneController[] redDrones = new DroneController[3];
            DroneController[] blueDrones = new DroneController[3];

            for (int i = 0; i < 3; i++)
            {
                redDrones[i] = allDrones[i];
                blueDrones[i] = allDrones[i + 3];
            }

            redDroneComm.InitializeNetwork(redDrones, drone => drone.Id);
            blueDroneComm.InitializeNetwork(blueDrones, drone => drone.Id);

            // Initialize Graph Communication Networks (New)
            redDroneNetwork = new DroneNetworkCommunication(); // Create graph for red drones
            blueDroneNetwork = new DroneNetworkCommunication(); // Create graph for blue drones

            foreach (var drone in redDrones)
            {
                redDroneNetwork.AddDrone(drone); // Add red drones to the red graph
            }

            foreach (var drone in blueDrones)
            {
                blueDroneNetwork.AddDrone(drone); // Add blue drones to the blue graph
            }
        }
        else
        {
            Debug.LogWarning("Not enough drones to form two groups of three.");
        }
    }

    void Update()
    {
        // Trigger the split after exactly 10 seconds
        if (Time.time - lastPartitionTime >= partitionInterval)
        {
            lastPartitionTime = Time.time;
            ChangeDronesDirection();
        }

        foreach (var drone in allDrones)
        {
            Vector3 direction = drone.GetMovementDirection();
            drone.transform.Translate(direction * drone.speed * Time.deltaTime);
            CheckAndHandleBoundary(drone);
        }

        // Example: Testing the communication network
        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to test red network
        {
            redDroneNetwork.SendMessage(allDrones[0], allDrones[1], "Message to Red Partition!");
        }

        if (Input.GetKeyDown(KeyCode.B)) // Press 'B' to test blue network
        {
            blueDroneNetwork.SendMessage(allDrones[3], allDrones[4], "Message to Blue Partition!");
        }
    }

    private void ChangeDronesDirection()
    {
        for (int i = 0; i < 3; i++)
        {
            allDrones[i].SetMovementDirection(Vector3.back); // Red drones now move backward
        }

        for (int i = 3; i < 6; i++)
        {
            allDrones[i].SetMovementDirection(Vector3.left); // Blue drones now move left
        }

        Debug.Log("Red and blue drones have changed directions.");
    }

    private void CheckAndHandleBoundary(DroneController drone)
    {
        Vector3 dronePosition = drone.transform.position;

        if (dronePosition.x >= xBoundaryMax && drone.GetMovementDirection() == Vector3.right)
        {
            drone.SetMovementDirection(Vector3.left);
        }
        else if (dronePosition.x <= xBoundaryMin && drone.GetMovementDirection() == Vector3.left)
        {
            drone.SetMovementDirection(Vector3.right);
        }

        if (dronePosition.z >= zBoundaryMax && drone.GetMovementDirection() == Vector3.forward)
        {
            drone.SetMovementDirection(Vector3.back);
        }
        else if (dronePosition.z <= zBoundaryMin && drone.GetMovementDirection() == Vector3.back)
        {
            drone.SetMovementDirection(Vector3.forward);
        }
    }
}
