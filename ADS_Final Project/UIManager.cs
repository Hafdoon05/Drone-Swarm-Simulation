using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public TMP_InputField droneIdInputField;
    public Button searchButton;
    public Button selfDestructButton;

    public Button shortestPathButton;
    public TMP_Text resultText;
    public DroneBTCommunication redDroneBTComm;
    public DroneBTCommunication blueDroneBTComm;
    public TMP_InputField startDroneIdInputField;
    public TMP_InputField endDroneIdInputField;


    void Start()
    {
        searchButton.onClick.AddListener(OnSearchButtonClicked);
        selfDestructButton.onClick.AddListener(OnSelfDestructButtonClicked);
        shortestPathButton.onClick.AddListener(OnShortestPathButtonClicked);
    }

    void OnSearchButtonClicked()
    {
        if (int.TryParse(droneIdInputField.text, out int droneId))
        {
            float totalTimeRed = redDroneBTComm.SearchDrone(droneId, d => d.Id);
            float totalTimeBlue = blueDroneBTComm.SearchDrone(droneId, d => d.Id);

            if (totalTimeRed >= 0)
            {
                resultText.text = $"Drone {droneId} found in red network.\nTotal time: {totalTimeRed:F2}";
            }
            else if (totalTimeBlue >= 0)
            {
                resultText.text = $"Drone {droneId} found in blue network.\nTotal time: {totalTimeBlue:F2}";
            }
            else
            {
                resultText.text = $"Drone {droneId} not found in any network.";
            }
        }
        else
        {
            resultText.text = "Invalid drone ID entered.";
        }
    }

    void OnSelfDestructButtonClicked()
    {
        if (int.TryParse(droneIdInputField.text, out int droneId))
        {
            float totalTimeRed = redDroneBTComm.SendSelfDestruct(droneId, d => d.Id);
            float totalTimeBlue = blueDroneBTComm.SendSelfDestruct(droneId, d => d.Id);

            if (totalTimeRed >= 0)
            {
                resultText.text = $"Drone {droneId} self-destructed in red network.\nTotal time: {totalTimeRed:F2}";
            }
            else if (totalTimeBlue >= 0)
            {
                resultText.text = $"Drone {droneId} self-destructed in blue network.\nTotal time: {totalTimeBlue:F2}";
            }
            else
            {
                resultText.text = $"Drone {droneId} not found in any network.";
            }
        }
        else
        {
            resultText.text = "Invalid drone ID entered.";
        }
    }

    void OnShortestPathButtonClicked()
{
    if (int.TryParse(startDroneIdInputField.text, out int startDroneId) &&
        int.TryParse(endDroneIdInputField.text, out int endDroneId))
    {
        // Check if drones are in the red network
        DroneController redStart = redDroneBTComm.FindDrone(startDroneId, d => d.Id);
        DroneController redEnd = redDroneBTComm.FindDrone(endDroneId, d => d.Id);

        // Check if drones are in the blue network
        DroneController blueStart = blueDroneBTComm.FindDrone(startDroneId, d => d.Id);
        DroneController blueEnd = blueDroneBTComm.FindDrone(endDroneId, d => d.Id);

        if (redStart != null && redEnd != null)
        {
            // Both drones are in the red network
            float redDistance = redDroneBTComm.FindShortestPath(startDroneId, endDroneId, d => d.Id);
            resultText.text = $"Shortest path in red network: {redDistance:F2} units.";
        }
        else if (blueStart != null && blueEnd != null)
        {
            // Both drones are in the blue network
            float blueDistance = blueDroneBTComm.FindShortestPath(startDroneId, endDroneId, d => d.Id);
            resultText.text = $"Shortest path in blue network: {blueDistance:F2} units.";
        }
        else if (redStart != null && blueEnd != null)
        {
            // Start in red, end in blue
            float crossDistance = Vector3.Distance(redStart.transform.position, blueEnd.transform.position);
            resultText.text = $"Shortest path between networks: {crossDistance:F2} units.";
        }
        else if (blueStart != null && redEnd != null)
        {
            // Start in blue, end in red
            float crossDistance = Vector3.Distance(blueStart.transform.position, redEnd.transform.position);
            resultText.text = $"Shortest path between networks: {crossDistance:F2} units.";
        }
        else
        {
            resultText.text = "Drones not found in any network.";
        }
    }
    else
    {
        resultText.text = "Invalid start or end Drone IDs.";
    }
}

}
