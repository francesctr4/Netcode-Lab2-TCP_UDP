using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;

    public GameObject UItextObj;
    public TMP_InputField messageInputField; // Add this for the input field
    public TextMeshProUGUI UItext;

    string serverText;
    List<User> connectedUsers = new List<User>();
    object lockObj = new object();

    private string serverName;
    private string hostName;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        serverName = PlayerPrefs.GetString("ServerName", "...");
        hostName = PlayerPrefs.GetString("HostName");
    }

    void Update()
    {
        lock (lockObj)
        {
            UItext.text = serverText;
        }
    }

    public void startServer()
    {
        serverText += " -------- STARTING TCP SERVER: " + serverName + " -------- " + "\n";
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket.Bind(ipep);
        socket.Listen(10);

        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            User newUser = new User();
            newUser.socket = socket.Accept(); // Accept new connection
            lock (lockObj)
            {
                connectedUsers.Add(newUser); // Add new user to the list
                SendMessageFromServer(" ---- USER JOINED SERVER: " + serverName + " ---- ", false);
                serverText += " -------- USER JOINED -------- " + "\n";
            }
            Thread clientThread = new Thread(() => Receive(newUser));
            clientThread.Start();
        }
    }

    void Receive(User user)
    {
        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            try
            {
                recv = user.socket.Receive(data);
                if (recv == 0)
                    break;

                string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                SendMessageFromServer(receivedMessage);
            }
            catch (SocketException)
            {
                break;
            }
        }

        lock (lockObj)
        {
            connectedUsers.Remove(user); // Remove disconnected user
        }
        user.socket.Close();
    }

    void MyBroadcastMessage(User sender, string message)
    {
        if (sender.socket != null)
        {
            byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
            sender.socket.Send(messageBuffer);
        }
    }

    public void SendMessageFromServer(string message, bool server = true)
    {
        if (server)
        {
            lock (lockObj)
            {
                serverText += message + "\n";
            }
        }
        
        // Broadcast the message to all connected clients
        foreach (var user in connectedUsers)
        {
            MyBroadcastMessage(user, message);
        }
    }

    // Call this function when the send button is clicked
    public void OnSendButtonClick()
    {
        string message = messageInputField.text; // Get the message from the input field
        if (!string.IsNullOrEmpty(message))
        {
            SendMessageFromServer(hostName + ": " + message); // Send the message
            messageInputField.text = ""; // Clear the input field after sending
        }
    }
}
