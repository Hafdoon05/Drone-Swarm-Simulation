using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_InputField droneIdInputField;
    public Button searchButton;
    public Button selfDestructButton;
    public TMP_Text resultText;
    public TMP_Text fpsText;

    public DroneCommunication redDroneComm;
    public DroneCommunication blueDroneComm;

    void Start()
    {
        searchButton.onClick.AddListener(OnSearchButtonClicked);
        selfDestructButton.onClick.AddListener(OnSelfDestructButtonClicked);
    }

    void OnSearchButtonClicked()
    {
        int droneId;
        if (int.TryParse(droneIdInputField.text, out droneId))
        {
            float totalTimeRed = redDroneComm.SearchDrone(droneId);
            float totalTimeBlue = blueDroneComm.SearchDrone(droneId);

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
        int droneId;
        if (int.TryParse(droneIdInputField.text, out droneId))
        {
            float totalTimeRed = redDroneComm.SendSelfDestruct(droneId);
            float totalTimeBlue = blueDroneComm.SendSelfDestruct(droneId);

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
