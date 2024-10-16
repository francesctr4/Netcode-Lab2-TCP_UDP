using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientTCP : MonoBehaviour
{
    Socket server;

    public GameObject UItextObj;  
    public TMP_InputField messageInputField; 
    public TextMeshProUGUI UItext;

    private string clientText = "";
    private string serverIPAddress;
    private string clientName; 

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverIPAddress = GetClientInputIPAddress();
        clientName = GetClientName(); // Set client name for this session
    }

    private string GetClientName()
    {
        // You can get this from an input field or generate a random name
        return PlayerPrefs.GetString("ClientName", "Client" + UnityEngine.Random.Range(1, 1000));
    }

    void Update()
    {
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
        //Thread sendThread = new Thread(Send); 
        //sendThread.Start();

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    public void OnSendButtonClick()
    {
        string message = messageInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            SendMessageToServer(message);
            messageInputField.text = ""; // Clear the input field after sending the message
        }
    }

    void SendMessageToServer(string message)
    {
        byte[] messageBuffer = Encoding.ASCII.GetBytes(clientName + ": " + message);
        server.Send(messageBuffer);
    }

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

    public static string GetClientInputIPAddress()
    {
        return PlayerPrefs.GetString("ServerIP", "127.0.0.1"); // Default to localhost if no IP is set
    }
}
