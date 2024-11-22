using System.Collections.Generic;
using UnityEngine;

public class DroneNetworkCommunication
{
    private Dictionary<DroneController, List<DroneController>> graph;

    // Constructor: Initializes the graph
    public DroneNetworkCommunication()
    {
        graph = new Dictionary<DroneController, List<DroneController>>();
    }

    // Add a drone to the graph and connect it to all other drones in the network
    public void AddDrone(DroneController drone)
    {
        if (!graph.ContainsKey(drone))
        {
            graph[drone] = new List<DroneController>();

            // Connect this drone to all existing drones
            foreach (var otherDrone in graph.Keys)
            {
                graph[drone].Add(otherDrone); // Connect the new drone to existing drones
                graph[otherDrone].Add(drone); // Connect existing drones to the new drone
            }
        }
    }

    // Get the neighbors (connections) of a specific drone
    public List<DroneController> GetConnections(DroneController drone)
    {
        if (graph.ContainsKey(drone))
        {
            return graph[drone];
        }
        return new List<DroneController>(); // Return an empty list if the drone is not in the graph
    }

    // Send a message to a specific drone via the graph network
    public void SendMessage(DroneController startDrone, DroneController targetDrone, string message)
    {
        if (!graph.ContainsKey(startDrone) || !graph.ContainsKey(targetDrone))
        {
            Debug.Log("Message cannot be sent: One or both drones are not in the network.");
            return;
        }

        Debug.Log($"Message '{message}' sent from Drone {startDrone.Id} to Drone {targetDrone.Id}.");
    }
}
