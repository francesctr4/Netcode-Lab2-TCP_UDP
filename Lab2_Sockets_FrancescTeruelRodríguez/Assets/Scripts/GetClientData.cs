using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetClientData : MonoBehaviour
{
    public TMP_Text clientNameText;
    public TMP_Text serverIPText;

    void Start()
    {
        string clientName = PlayerPrefs.GetString("ClientName", "No Client");
        string serverIP = PlayerPrefs.GetString("ServerIP", "No IP");

        clientNameText.text = "Client Name: " + clientName;
        serverIPText.text = "Server IP: " + serverIP; 
    }
}
