using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

/* Accept the client's connection request, receive byte data from the client, and write it to a file. */
public class MyServer : MonoBehaviour
{
    static byte[] Buffer { get; set; }
    static Socket serverSck, accepted;
    IPEndPoint ipEndPoint;
    public static string ip = "127.0.0.1";
    public static int port = 13;
    public static string FileDir = "F:\\Tah-Da\\";

    // Start is called before the first frame update
    void Start()
    {
        // create IPEndPoint
        IPAddress ipAddress = IPAddress.Parse(ip);
        ipEndPoint = new IPEndPoint(ipAddress, port);
        // create socket
        serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSck.Bind(ipEndPoint);
        serverSck.Listen(1); // 链接等待队列：队列中最多有1个连接
        // use thread to carry out async behaviors
        Thread connectionThread = new Thread(OnConnection);
        connectionThread.Start();
    }

    void OnConnection()
    {
        Debug.Log("Server starts accepting client's connection requests...");
        accepted = serverSck.Accept(); // transfer the socket trying to connect to the variable.
        Debug.Log("Connected client: " + accepted.RemoteEndPoint.ToString());
        receiveMessage();
        closeConnection();
    }

    void receiveMessage()
    {
        // get the total number of bytes
        Buffer = new byte[accepted.SendBufferSize];
        accepted.Receive(Buffer);
        int totalBytes = int.Parse(Encoding.UTF8.GetString(Buffer));
        Debug.Log("Number of Bytes: " + totalBytes);
        // receive data according to the number of bytes
        string strData = "";
        while (totalBytes != 0)
        {
            Buffer = new byte[accepted.SendBufferSize]; // clear the buffer
            int bytesRead = accepted.Receive(Buffer); // store the message in the Buffer, return the number of bytes
            // format bytesRead, since it will fill the unfilled bytes with blank
            byte[] formatted = new byte[bytesRead];
            for (int i = 0; i < bytesRead; i++)
            {
                formatted[i] = Buffer[i];
            }
            strData += Encoding.UTF8.GetString(formatted);
            totalBytes -= bytesRead;
        }
        stringToFile(strData); // write to file
        Debug.Log("Received: " + strData); // output the data to console
    }

    void stringToFile(string content)
    {
        using (StreamWriter writer = new StreamWriter(FileDir + "TestOut.obj"))
        {
            writer.Write(content);
        }
    }

    void closeConnection()
    {
        serverSck.Close();
        accepted.Close();
        Debug.Log("Connection closed. ");
    }
}
