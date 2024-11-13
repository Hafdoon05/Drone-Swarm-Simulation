using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_InputField droneIdInputField;
    public Button searchButton;
    public Button selfDestructButton;
    public TMP_Text resultText;
    public DroneBTCommunication redDroneBTComm;
    public DroneBTCommunication blueDroneBTComm;

    void Start()
    {
        searchButton.onClick.AddListener(OnSearchButtonClicked);
        selfDestructButton.onClick.AddListener(OnSelfDestructButtonClicked);
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
}
