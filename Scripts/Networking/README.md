`ClayClient.cs`：挂载在Unity场景中，作为通信的Client。其中调用`HoloLensClient`以实现Socket通信，需要与`HoloLensClient`放在同一目录。

`ServerMain.cs`：在Linux中运行，作为通信的Server，命令行`dotnet run`以启动。其中调用`LinuxServer`以实现Socket通信，需要与`LinuxServer`放在同一目录。

`HoloLensClient.cs`和`LinuxServer.cs`：分别定义namespace，提供可用的函数。通过`HLClient`和`LinServer`类调用。

`command.cs`：提供C#调用命令行的helper method。

`MyClient.cs`和`MyServer.cs`：可以直接挂载在Unity场景中运行的通信脚本。最早的测试版。