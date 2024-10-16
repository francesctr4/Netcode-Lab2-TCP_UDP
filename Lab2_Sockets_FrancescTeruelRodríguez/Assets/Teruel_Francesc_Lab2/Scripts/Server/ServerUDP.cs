﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections.Generic;

public class ServerUDP : MonoBehaviour
{
    private Socket socket;

    public GameObject UItextObj;
    public TMP_InputField messageInputField;
    public TextMeshProUGUI UItext;

    private string serverText;
    private string serverName;
    private string hostName;

    private Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>(); // Store client IP and names

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverName = PlayerPrefs.GetString("ServerName", "...");
        hostName = PlayerPrefs.GetString("HostName", "Server");
    }

    public void startServer()
    {
        serverText += " -------- STARTING UDP SERVER: " + serverName + " -------- " + "\n";
        UItext.text = serverText;

        int port = 9050;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        // Start the thread to receive messages
        Thread newConnection = new Thread(Receive);
        newConnection.Start();
    }

    void Update()
    {
        UItext.text = serverText;
    }

    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)sender;

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref remote);
            string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);

            // Add client to the list if it's not already there
            if (!connectedClients.ContainsKey((IPEndPoint)remote))
            {
                connectedClients[(IPEndPoint)remote] = "Client_" + connectedClients.Count; // Assign a default name or handle it differently
                SendMessageToAll($" ---- USER JOINED SERVER: {serverName} ---- ");
            }

            ProcessMessage((IPEndPoint)remote, receivedMessage);
        }
    }

    void ProcessMessage(IPEndPoint sender, string message)
    {
        SendMessageToAll($"{message}");
    }

    void SendMessageToAll(string message)
    {
        serverText += message + "\n";

        byte[] data = Encoding.ASCII.GetBytes(message);
        foreach (var client in connectedClients.Keys)
        {
            socket.SendTo(data, data.Length, SocketFlags.None, client);
        }
    }

    public void OnSendButtonClick()
    {
        string message = messageInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            // Send the message to all connected clients
            SendMessageToAll($"{hostName}: {message}");
            messageInputField.text = ""; // Clear the input field after sending
        }
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    public void StopServer()
    {
        // Close the UDP socket
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }

        serverText += " -------- UDP SERVER SHUT DOWN -------- " + "\n";
    }

}
