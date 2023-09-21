using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class SocketClient : MonoBehaviour
{
    public TextMeshProUGUI[] tmpT = new TextMeshProUGUI[4];
    private Socket clientSocket;
    private byte[] buffer = new byte[1024];

    private bool isRunning = true;
    private float sendInterval = 0.01f; // Send data every 0.1 seconds
    private float timer = 0f;

    void Start()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress serverIP = IPAddress.Parse("192.168.100.81");
        int serverPort = 12345;

        clientSocket.Connect(new IPEndPoint(serverIP, serverPort));
    }

    void Update()
    {
        // Check the timer and send data if enough time has elapsed
        timer += Time.deltaTime;
        if (timer >= sendInterval)
        {
            try
            {
                string messageToSend = "";

                for (int i = 0; i < 3; i++)
                {
                    messageToSend = messageToSend + tmpT[i].text.ToString() + ",";
                }
                messageToSend = messageToSend + tmpT[3].text.ToString();

                byte[] messageBytes = Encoding.ASCII.GetBytes(messageToSend);

                clientSocket.Send(messageBytes);
                int bytesRead = clientSocket.Receive(buffer);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Debug.Log("Received from server: " + receivedMessage);

                timer = 0f; // Reset the timer
            }
            catch (Exception e)
            {
                Debug.LogError("Socket error: " + e.ToString());
            }
        }
    }

    void OnDestroy()
    {
        isRunning = false;
        
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
