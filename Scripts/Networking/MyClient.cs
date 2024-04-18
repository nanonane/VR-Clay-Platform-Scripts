using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

public class MyClient : MonoBehaviour
{
    static Socket sck;
    IPEndPoint ipEndPoint;
    NetworkStream stream;
    public static string ip = "127.0.0.1";
    public static int port = 13;
    public static string FileDir = "F:\\sample_meshes\\";

    // Start is called before the first frame update
    void Start()
    {
        // create IPEndPoint
        IPAddress ipAddress = IPAddress.Parse(ip);
        ipEndPoint = new IPEndPoint(ipAddress, port);
        // create socket
        sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        attemptConnection();

    }

    void attemptConnection()
    {
        // connect
        try
        {
            sck.Connect(ipEndPoint);
        }
        catch
        {
            Debug.Log("Unable to connect to remote end point! \n");
            return;
        }
        Debug.Log("Connected to the server. ");
        sendMessage();
        closeConnection();
    }

    void sendMessage()
    {
        // prepare message
        // string text = "Hello there!";
        string text = stringFromFile();
        byte[] data = Encoding.UTF8.GetBytes(text);

        // send the number of bytes to the server, in case the data length exceeds the size of the buffer. 
        sck.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
        Debug.Log("Number of bytes: " + data.Length.ToString());

        // send data
        sck.Send(data);
        Debug.Log("Data Sent!\n");
    }

    string stringFromFile()
    {
        string content = "";
        using (StreamReader reader = File.OpenText(FileDir + "bunny10k.obj"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                content += line;
                content += "\n";
            }
        }
        return content;
    }

    void closeConnection()
    {
        sck.Close();
        Debug.Log("Connection closed. ");
    }
}
