`ClayClient`：挂载在Unity场景中，作为通信的Client。其中调用`HoloLensClient`以实现Socket通信，需要与`HoloLensClient`放在同一目录。

`ServerMain`：在Linux中运行，作为通信的Server，命令行`dotnet run`以启动。其中调用`WindowsServer`以实现Socket通信，需要与`WindowsServer`放在同一目录。

`HoloLensClient`和`WindowsServer`：分别定义namespace，提供可用的函数。通过`HLClient`和`WinServer`类调用。

`MyClient`和`MyServer`：可以直接挂载在Unity场景中运行的通信脚本。最早的测试版。