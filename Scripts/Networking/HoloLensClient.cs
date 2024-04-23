using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace HoloLensClient
{
    public class HLClient
    {
        static byte[] Buffer { get; set; }
        static Socket sck;
        IPEndPoint ipEndPoint;
        NetworkStream stream;
        public static string ip = "172.22.96.132";
        public static int port = 14159;
        public static string FileDir = "F:\\sample_test_output\\TestCl\\";

        public void setUp()
        {
            // create IPEndPoint
            IPAddress ipAddress = IPAddress.Parse(ip);
            ipEndPoint = new IPEndPoint(ipAddress, port);
            // create socket
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void attemptConnection()
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
        }

        public void sendFileInUnity()
        {
            // prepare message
            string text = stringFromFile();
            byte[] data = Encoding.UTF8.GetBytes(text);

            // send the number of bytes to the server, in case the data length exceeds the size of the buffer. 
            byte[] number = new byte[sck.SendBufferSize];
            string dataLen = data.Length.ToString();
            for (int i = 0; i < dataLen.Length; i++)
            {
                number[i] = Convert.ToByte(dataLen[i]);
            }
            sck.Send(number);
            Debug.Log("Number of bytes: " + dataLen);

            // send data
            sck.Send(data);
            Debug.Log("Data Sent!\n");
        }

        public void sendMessage()
        {
            // prepare message
            string text = stringFromFile();
            byte[] data = Encoding.UTF8.GetBytes(text);

            // send data
            sck.Send(data);
            Debug.Log("Data Sent!\n");
        }

        string stringFromFile()
        {
            string content = "";
            using (StreamReader reader = File.OpenText(FileDir + "HandPosition.txt"))
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

        public void receiveMessage()
        {
            // get the total number of bytes
            Buffer = new byte[sck.SendBufferSize];
            sck.Receive(Buffer);
            int totalBytes = int.Parse(Encoding.UTF8.GetString(Buffer));
            Debug.Log("Number of Bytes: " + totalBytes);
            // receive data according to the number of bytes
            string strData = "";
            while (totalBytes != 0)
            {
                Buffer = new byte[sck.SendBufferSize]; // clear the buffer
                int bytesRead = sck.Receive(Buffer); // store the message in the Buffer, return the number of bytes
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
            sck.Close();
            Debug.Log("Connection closed. ");
        }
    }

}
