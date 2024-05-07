using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace LinuxServer
{
    public class LinServer
    {

        static byte[] Buffer { get; set; }
        static Socket serverSck, accepted;
        IPEndPoint ipEndPoint;
        static int countTurns = 0;
        public static string ip = "172.22.96.132";
        public static int port = 14159;
        public static string FileDir = "/home/zhuyinuo/TransFiles/";

        public void setUp()
        {
            // create IPEndPoint
            IPAddress ipAddress = IPAddress.Parse(ip);
            ipEndPoint = new IPEndPoint(ipAddress, port);
            // create socket
            serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSck.Bind(ipEndPoint);
            serverSck.Listen(1); // at most one connection in the queue
        }

        public void OnConnection()
        {
            Console.WriteLine("Server starts accepting client's connection requests...");
            accepted = serverSck.Accept(); // transfer the socket trying to connect to the variable.
            Console.WriteLine("Connected client: " + accepted.RemoteEndPoint.ToString());
        }

        public void sendMessage()
        {
            // prepare message
            string text = stringFromFile("hero_" + countTurns);
            byte[] data = Encoding.UTF8.GetBytes(text);

            // send the number of bytes to the server, in case the data length exceeds the size of the buffer. 
            accepted.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
            Console.WriteLine("Number of bytes: " + data.Length.ToString());

            // send data
            accepted.Send(data);
            Console.WriteLine("Data Sent!\n");
            countTurns += 1;
        }

        string stringFromFile(string objName)
        {
            string content = "";
            using (StreamReader reader = File.OpenText(FileDir + "output/clay/" + objName + ".obj"))
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
            Console.WriteLine("Start receiving messages...");
            Buffer = new byte[accepted.SendBufferSize]; // clear the buffer
            int bytesRead = accepted.Receive(Buffer); // store the message in the Buffer, return the number of bytes
                                                      // format bytesRead, since it will fill the unfilled bytes with blank

            byte[] formatted = new byte[bytesRead];
            for (int i = 0; i < bytesRead; i++)
            {
                formatted[i] = Buffer[i];
            }
            Console.WriteLine(bytesRead);
            string strData = Encoding.UTF8.GetString(formatted);
            stringToFile(strData); // write to file
            Console.WriteLine("Received: " + strData); // output the data to console
        }

        void stringToFile(string content)
        {
            using (StreamWriter writer = new StreamWriter(FileDir + "hand.txt"))
            {
                writer.Write(content);
            }
        }

        public void closeConnection()
        {
            serverSck.Close();
            accepted.Close();
            Console.WriteLine("Connection closed. ");
        }
    }

}
