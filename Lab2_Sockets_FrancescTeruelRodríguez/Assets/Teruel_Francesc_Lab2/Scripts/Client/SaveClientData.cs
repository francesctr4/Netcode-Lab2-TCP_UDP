using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveClientData : MonoBehaviour
{
    public TMP_InputField clientNameInput;
    public TMP_InputField serverIPInput; 

    public void OnJoinServerButtonPressed() // Called when the Join Server button is pressed
    {
        string clientName = clientNameInput.text;
        string serverIP = serverIPInput.text; 

        PlayerPrefs.SetString("ClientName", clientName); 
        PlayerPrefs.SetString("ServerIP", serverIP); 

        PlayerPrefs.Save(); // Make sure to save the PlayerPrefs to use them later on other scenes
    }
}
