using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    private Socket server;
    public GameObject UItextObj;
    public TMP_InputField messageInputField;
    private TextMeshProUGUI UItext;

    private string clientText = "";
    private string serverIPAddress;
    private string clientName;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverIPAddress = GetClientInputIPAddress();
        clientName = GetClientName(); // Set client name for this session
    }

    // Function to get the client name
    private string GetClientName()
    {
        // You can get this from an input field or generate a random name
        return PlayerPrefs.GetString("ClientName", "Client" + UnityEngine.Random.Range(1, 1000));
    }

    public void StartClient()
    {
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText; // Display received messages
    }

    void Send()
    {
        int port = 9050;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIPAddress), port);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Send UDP handshake message to the server
        string handshake = "Connecting...";
        byte[] data = Encoding.ASCII.GetBytes(handshake);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        Debug.Log("Sent handshake message");

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    public void OnSendButtonClick()
    {
        string message = clientName + ": " + messageInputField.text; // Get the text from the input field
        if (!string.IsNullOrEmpty(message))
        {
            SendMessageToServer(message);
            messageInputField.text = ""; // Clear the input field after sending
        }
    }

    void SendMessageToServer(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), 9050);
        server.SendTo(data, data.Length, SocketFlags.None, endpoint); // Send message to the server
    }

    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)(sender);
        byte[] data = new byte[1024];

        while (true)
        {
            int recv = server.ReceiveFrom(data, ref remote);
            string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
            lock (clientText) // Ensure thread safety
            {
                clientText += $"{receivedMessage}\n";
            }
        }
    }

    public static string GetClientInputIPAddress()
    {
        return PlayerPrefs.GetString("ServerIP", "No IP");
    }
}

