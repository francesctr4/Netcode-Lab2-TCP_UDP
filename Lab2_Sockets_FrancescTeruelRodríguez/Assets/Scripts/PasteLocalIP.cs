using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;

public class PasteLocalIP : MonoBehaviour
{
    private string localIPAddress;
    public TMP_InputField IPInputText;

    // Start is called before the first frame update
    void Start()
    {
        localIPAddress = GetLocalIPAddress();
    }

    public void OnButtonClick()
    {
        IPInputText.text = localIPAddress;
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
