using System;
using UnityEngine;

public class DroneBTCommunication : MonoBehaviour
{
    private DroneNode root;

    private class DroneNode
    {
        public DroneController drone;
        public DroneNode left, right;

        public DroneNode(DroneController drone)
        {
            this.drone = drone;
            left = right = null;
        }
    }

    public void InitializeNetwork(DroneController[] drones, Func<DroneController, int> keySelector)
    {
        foreach (var drone in drones)
        {
            root = Insert(root, drone, keySelector);
        }
    }

    private DroneNode Insert(DroneNode node, DroneController drone, Func<DroneController, int> keySelector)
    {
        if (node == null) return new DroneNode(drone);

        int key = keySelector(drone);
        int nodeKey = keySelector(node.drone);

        if (key < nodeKey)
            node.left = Insert(node.left, drone, keySelector);
        else if (key > nodeKey)
            node.right = Insert(node.right, drone, keySelector);

        return node;
    }

    public float SearchDrone(int droneId, Func<DroneController, int> keySelector)
    {
        float totalTime = 0f;
        var foundDrone = Search(root, droneId, ref totalTime, keySelector);
        if (foundDrone != null)
        {
            Debug.Log($"Drone {droneId} found at position {foundDrone.transform.position}");
        }
        else
        {
            Debug.Log($"Drone {droneId} not found in network.");
        }
        return foundDrone != null ? totalTime : -1f;
    }

    private DroneController Search(DroneNode node, int droneId, ref float totalTime, Func<DroneController, int> keySelector)
    {
        if (node == null) return null;

        if (node.drone.Id == droneId) return node.drone;

        DroneNode nextNode = (droneId < keySelector(node.drone)) ? node.left : node.right;
        if (nextNode != null)
            totalTime += Vector3.Distance(node.drone.transform.position, nextNode.drone.transform.position);

        return Search(nextNode, droneId, ref totalTime, keySelector);
    }

    public float SendSelfDestruct(int droneId, Func<DroneController, int> keySelector)
    {
        float totalTime = 0f;
        root = DeleteNode(root, droneId, ref totalTime, keySelector);
        return totalTime;
    }

    private DroneNode DeleteNode(DroneNode node, int droneId, ref float totalTime, Func<DroneController, int> keySelector)
    {
        if (node == null) return null;

        if (node.drone.Id == droneId)
        {
            node.drone.gameObject.SetActive(false);
            Debug.Log($"Drone {droneId} has been self-destructed.");

            if (node.left == null) return node.right;
            if (node.right == null) return node.left;

            DroneNode minNode = GetMinNode(node.right);
            node.drone = minNode.drone;
            node.right = DeleteNode(node.right, minNode.drone.Id, ref totalTime, keySelector);
        }
        else if (droneId < keySelector(node.drone))
        {
            node.left = DeleteNode(node.left, droneId, ref totalTime, keySelector);
        }
        else
        {
            node.right = DeleteNode(node.right, droneId, ref totalTime, keySelector);
        }

        return node;
    }

    private DroneNode GetMinNode(DroneNode node)
    {
        while (node.left != null)
        {
            node = node.left;
        }
        return node;
    }

    internal void InitializeNetwork(DroneController[] redDrones)
    {
        throw new NotImplementedException();
    }
}