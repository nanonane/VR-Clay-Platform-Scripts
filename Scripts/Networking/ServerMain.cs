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
    server.OnConnection();
    server.receiveMessage();
    server.sendMessage();
    server.closeConnection();
    return 0;
}

main();
