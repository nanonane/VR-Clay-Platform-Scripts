using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using WindowsServer;
using ShellCommand;

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
                @"cd ~/repos/IDP/Projects/FEM/our_scripts
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
