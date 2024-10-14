using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveServerData : MonoBehaviour
{
    public TMP_InputField hostNameInput; // Reference to the TextMesh Pro input field
    public TMP_InputField serverNameInput; // Reference to the TextMesh Pro input field

    public void OnCreateServerButtonPressed() // This should be called when the connect button is pressed
    {
        string hostName = hostNameInput.text;
        string serverName = serverNameInput.text; // Get the server name from the input field

        PlayerPrefs.SetString("HostName", hostName); // Save the server name for later use
        PlayerPrefs.SetString("ServerName", serverName); // Save the server name for later use
        PlayerPrefs.SetString("ServerIP", GetLocalIPAddress()); // Save the server name for later use

        PlayerPrefs.Save(); // Make sure to save the PlayerPrefs
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            // We are looking for an IPv4 address (since most networks use IPv4)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No IPv4 address found for this machine.");
    }
}
