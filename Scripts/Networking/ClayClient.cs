using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using HoloLensClient;

public class ClayClient : MonoBehaviour
{
    HLClient client = new HLClient();
    // Start is called before the first frame update
    void Start()
    {
        client.setUp(); // 初始化
        client.attemptConnection(); // 连接到服务器
        client.sendMessage(); // 发送手势追踪结果的txt
        Thread receiveThread = new Thread(client.receiveMessage); // 开启接收文件的线程
        receiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
