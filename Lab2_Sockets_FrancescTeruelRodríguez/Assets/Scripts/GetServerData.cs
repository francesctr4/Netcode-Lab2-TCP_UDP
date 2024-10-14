using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetServerData : MonoBehaviour
{
    public TMP_Text hostNameText; // Reference to the TextMesh Pro text object
    public TMP_Text serverNameText; // Reference to the TextMesh Pro text object
    public TMP_Text serverIPText; // Reference to the TextMesh Pro text object

    void Start()
    {
        string hostName = PlayerPrefs.GetString("HostName", "No Host");
        string serverName = PlayerPrefs.GetString("ServerName", "No Server");
        string serverIP = PlayerPrefs.GetString("ServerIP", "No IP");

        hostNameText.text = "Host Name: " + hostName; // Display the server name
        serverNameText.text = "Server Name: " + serverName; // Display the server name
        serverIPText.text = "Server IP: " + serverIP; // Display the server name
    }
}
