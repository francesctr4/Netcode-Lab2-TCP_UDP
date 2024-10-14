using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System;

public class ClientUDP : MonoBehaviour
{
    Socket server;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;
    string serverIPAddress;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverIPAddress = GetClientInputIPAddress();
    }
    public void StartClient()
    {
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        //TO DO 2
        //Unlike with TCP, we don't "connect" first,
        //we are going to send a message to establish our communication so we need an endpoint
        //We need the server's IP and the port we've binded it to before
        //Again, initialize the socket
        
        int port = 9050;
        string localIP = serverIPAddress;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(localIP), port);

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //TO DO 2.1 
        //Send the Handshake to the server's endpoint.
        //This time, our UDP socket doesn't have it, so we have to pass it
        //as a parameter on it's SendTo() method

        byte[] data = new byte[1024];
        string handshake = "Hello World";

        data = Encoding.ASCII.GetBytes(handshake);

        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        Debug.Log("sent message");

        //TO DO 5
        //We'll wait for a server response,
        //so you can already start the receive thread
        Thread receive = new Thread(Receive);
        receive.Start();

    }

    //TO DO 5
    //Same as in the server, in this case the remote is a bit useless
    //since we already know it's the server who's communicating with us
    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        clientText = clientText += $"Message received from {Remote.ToString()}:" + "\n" + Encoding.ASCII.GetString(data, 0, recv) + "\n";
    }

    public static string GetClientInputIPAddress()
    {
        return PlayerPrefs.GetString("ServerIP", "No IP");
    }

}

