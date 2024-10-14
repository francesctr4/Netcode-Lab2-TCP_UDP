using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveClientData : MonoBehaviour
{
    public TMP_InputField clientNameInput; // Reference to the TextMesh Pro input field
    public TMP_InputField serverIPInput; // Reference to the TextMesh Pro input field

    public void OnJoinServerButtonPressed() // This should be called when the connect button is pressed
    {
        string clientName = clientNameInput.text;
        string serverIP = serverIPInput.text; // Get the server name from the input field

        PlayerPrefs.SetString("ClientName", clientName); // Save the server name for later use
        PlayerPrefs.SetString("ServerIP", serverIP); // Save the server name for later use

        PlayerPrefs.Save(); // Make sure to save the PlayerPrefs
    }
}
