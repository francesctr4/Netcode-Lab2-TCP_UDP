using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System;

public class ClientTCP : MonoBehaviour
{
    public GameObject UItextObj;  // Reference to the UI text object
    public TMP_InputField messageInputField; // Reference to the input field for sending messages
    public TextMeshProUGUI UItext; // TextMeshProUGUI component to display messages
    string clientText = "";
    Socket server;
    string serverIPAddress;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverIPAddress = GetClientInputIPAddress();
    }

    // Update is called once per frame
    void Update()
    {
        // Display the messages received from the server
        UItext.text = clientText;
    }

    // Starts the client and connects to the server
    public void StartClient()
    {
        Thread connectThread = new Thread(Connect);
        connectThread.Start();
    }

    // Establish a connection with the server
    void Connect()
    {
        int port = 9050;
        string localIP = serverIPAddress;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(localIP), port);

        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Connect(ipep);

        //// Start threads for sending and receiving messages
        //Thread sendThread = new Thread(Send); // Sending is manual, so this will wait for user input
        //sendThread.Start();

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    // Called when the Send button is clicked in the UI
    public void OnSendButtonClick()
    {
        // Get the text from the input field and send it to the server
        string message = messageInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            SendMessageToServer(message);
            messageInputField.text = ""; // Clear the input field after sending the message
        }
    }

    // Sends a custom message to the server
    void SendMessageToServer(string message)
    {
        byte[] messageBuffer = Encoding.ASCII.GetBytes(PlayerPrefs.GetString("ClientName") + ": " + message);
        server.Send(messageBuffer);
    }

    // Receives messages from the server
    void Receive()
    {
        byte[] data = new byte[1024];
        int recv;

        while (true)
        {
            try
            {
                recv = server.Receive(data);
                if (recv == 0)
                    break;

                string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                lock (clientText)
                {
                    clientText += receivedMessage + "\n";
                }
            }
            catch (SocketException)
            {
                // Handle any socket exceptions (e.g., server disconnect)
                break;
            }
        }

        server.Close();
    }

    // Retrieve the server's IP address from PlayerPrefs (set this in another part of your app)
    public static string GetClientInputIPAddress()
    {
        return PlayerPrefs.GetString("ServerIP", "127.0.0.1"); // Default to localhost if no IP is set
    }
}
