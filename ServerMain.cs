using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using WindowsServer;

int main()
{
    WinServer server = new WinServer();
    server.setUp();
    try
    {
        while (true)
        {
            server.OnConnection();
            while (true)
            {
                server.receiveMessage();
                server.sendMessage();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
    finally
    {
        server.closeConnection();
    }
    return 0;
}

main();
