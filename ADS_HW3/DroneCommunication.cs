using UnityEngine;
using System.Collections.Generic;

public class DroneCommunication : MonoBehaviour
{
    private DroneNode head;

    private class DroneNode
    {
        public DroneController drone;
        public DroneNode next;

        public DroneNode(DroneController drone)
        {
            this.drone = drone;
            next = null;
        }
    }

    public void InitializeNetwork(DroneController[] drones)
    {
        List<DroneNode> nodes = new List<DroneNode>();
        foreach (var drone in drones)
        {
            nodes.Add(new DroneNode(drone));
        }

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            nodes[i].next = nodes[i + 1];
        }
        nodes[nodes.Count - 1].next = null;
        head = nodes[0];
    }

    public float SearchDrone(int droneId)
    {
        float totalTime = 0f;
        DroneNode currentNode = head;

        while (currentNode != null)
        {
            if (currentNode.drone.Id == droneId)
            {
                Debug.Log("Drone " + droneId + " found at position " + currentNode.drone.transform.position);
                return totalTime;
            }

            if (currentNode.next != null)
            {
                float distance = Vector3.Distance(currentNode.drone.transform.position, currentNode.next.drone.transform.position);
                totalTime += distance;
            }

            currentNode = currentNode.next;
        }

        Debug.Log("Drone " + droneId + " not found in the network.");
        return -1f;
    }

    public float SendSelfDestruct(int droneId)
    {
        float totalTime = 0f;
        DroneNode currentNode = head;
        DroneNode previousNode = null;

        while (currentNode != null)
        {
            if (currentNode.drone.Id == droneId)
            {
                currentNode.drone.gameObject.SetActive(false);

                if (previousNode != null)
                {
                    previousNode.next = currentNode.next;
                }
                else
                {
                    head = currentNode.next;
                }

                Debug.Log("Drone " + droneId + " has been self-destructed.");
                return totalTime;
            }

            if (currentNode.next != null)
            {
                float distance = Vector3.Distance(currentNode.drone.transform.position, currentNode.next.drone.transform.position);
                totalTime += distance;
            }

            previousNode = currentNode;
            currentNode = currentNode.next;
        }

        Debug.Log("Drone " + droneId + " not found in the network.");
        return -1f;
    }
}
