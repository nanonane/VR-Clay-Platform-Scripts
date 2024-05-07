using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using LinuxServer;
using ShellCommand;

int main()
{
    LinServer server = new LinServer();
    server.setUp();
    try
    {
        while (true)
        {
            server.OnConnection();
            while (true)
            {
                server.receiveMessage();
                @"cd ~/IDP/Projects/FEM/scripts
                /usr/bin/python3.8 clay.py".Bash();
                server.sendMessage();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occured:" + ex.Message);
    }
    finally
    {
        server.closeConnection();
    }
    return 0;
}

main();
