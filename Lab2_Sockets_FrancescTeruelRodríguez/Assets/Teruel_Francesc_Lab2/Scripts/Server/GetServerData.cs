using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetServerData : MonoBehaviour
{
    public TMP_Text hostNameText;
    public TMP_Text serverNameText;
    public TMP_Text serverIPText;

    void Start()
    {
        string hostName = PlayerPrefs.GetString("HostName", "No Host");
        string serverName = PlayerPrefs.GetString("ServerName", "No Server");
        string serverIP = PlayerPrefs.GetString("ServerIP", "No IP");

        hostNameText.text = "Host Name: " + hostName; 
        serverNameText.text = "Server Name: " + serverName;
        serverIPText.text = "Server IP: " + serverIP; 
    }
}
