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
    public TMP_InputField hostNameInput;
    public TMP_InputField serverNameInput; 

    public void OnCreateServerButtonPressed()
    {
        string hostName = hostNameInput.text;
        string serverName = serverNameInput.text;

        PlayerPrefs.SetString("HostName", hostName); 
        PlayerPrefs.SetString("ServerName", serverName); 
        PlayerPrefs.SetString("ServerIP", GetLocalIPAddress());

        PlayerPrefs.Save(); // Make sure to save the PlayerPrefs
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            // We are looking for an IPv4 address, since most networks use IPv4.
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No IPv4 address found for this machine.");
    }
}
