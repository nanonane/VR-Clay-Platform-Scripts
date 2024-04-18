// // using LitJson
// using System.Collections;
// using System.Collections.Generic;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using UnityEngine.UI;
// using UnityEngine;
// using System;
// using System.IO;
 
// //Json数据
// [Serializable]
// public class JsonData
// {
//     public string operationType = string.Empty; //服务器事件类型（选择服务器要执行的方法）
//     public string clientIP = string.Empty;      //客户端本地ip
//     public string flieName = string.Empty;      //文件名称(文件名称)
//     public byte[] fileData = new byte[] { };    //文件byte[]数据
// }
 
// public class FileClient : MonoBehaviour
// {
//     private string myip = "127.0.0.1";
//     private int myPort = 5753;
//     JsonData jsonData;    //整合的json数据
 
//     public Button sendBtn;//发送
//     public InputField path;//信息
//     Socket clientSocket;
 
//     byte[] sendData; //要发送的数据
//     private static byte[] result = new byte[1024]; //接收服务器的数据
 
//     string filePath;
//     void Start()
//     {
//         // myip = ConfigFile.LoadString("Server");
//         // myPort = int.Parse(ConfigFile.LoadString("port"));
 
//         //按钮监听
//         sendBtn.onClick.AddListener(delegate {
 
//             //判断 路径不为空 并且 文件存在
//             if (!path.text.Equals(string.Empty)&& File.Exists(path.text)) 
//             {
               
//                 //判断 文件是否存在\反斜杠
//                 filePath = path.text.Contains("\\") ? path.text.Replace('\\', '/') : path.text;
//                 Debug.Log(filePath);
//                 try
//                 {
//                     //获取文件名称 类型
//                     FieldSplit(filePath, '/');
//                     jsonData = new JsonData()
//                     {
//                         operationType = "上传附件",
//                         clientIP = GetLocalIP.GetIP(GetLocalIP.ADDRESSFAM.IPv4),
//                         flieName = splitData[splitData.Length - 1],
//                         fileData = GetFileData(filePath) //将文件转换为byte[]
//                     };
 
//                     Debug.Log(jsonData.flieName);
 
//                     sendData = Encoding.UTF8.GetBytes(JsonMapper.ToJson(jsonData)); //将json数据转换为byte
 
//                     //1.先发送 数据长度
//                     clientSocket.Send(Encoding.UTF8.GetBytes(sendData.Length.ToString()));
//                     Debug.Log("byte数量：" + sendData.Length.ToString());
//                     //2.发送内容
//                     clientSocket.Send(sendData);//传送信息
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.Log("发送失败：" + ex);
//                 }
 
//             }
 
//         });
 
//         //监听连接的客户端连接
//         ListeningClient();
 
//     }
 
//     #region 获取文件名称 类型
//     string[] splitData; 
//     private void FieldSplit(string field,char _value)
//     {
//         splitData = field.Split(_value);
//     }
 
//     #endregion
 
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             Application.Quit();
//         }
//     }
 
//     #region 监听连接的客户端连接
//     private void ListeningClient() 
//     {
//         //要连接的服务器IP地址  
//         IPAddress ip = IPAddress.Parse(myip);//本地IP地址
//         clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//         try
//         {
//             clientSocket.Connect(new IPEndPoint(ip, myPort)); //配置服务器IP与端口 ，并且尝试连接
//         }
//         catch (Exception ex)
//         {
//             Debug.Log("Connect lose" + ex); //打印连接失败
//             return;
//         }
 
//         int receiveLength = clientSocket.Receive(result);//接收服务器回复消息，成功则说明已经接通
//         Debug.Log("服务器消息长度：" + receiveLength);
 
//         if (receiveLength > 1)
//         {
//             Debug.Log("接收服务器消息：" + Encoding.UTF8.GetString(result));
//         }
//     }
 
//     #endregion
 
 
//     #region 将文件转换成byte[] 数组
 
//     /// <summary>
//     /// 将文件转换成byte[] 数组
//     /// </summary>
//     /// <param name="fileUrl">文件路径文件名称</param>
//     /// <returns>byte[]</returns>
//     protected byte[] GetFileData(string fileUrl)
//     {
//         FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
//         lock (fs)
//         {
//             try
//             {
//                 byte[] buffur = new byte[fs.Length];
//                 fs.Read(buffur, 0, (int)fs.Length);
 
//                 return buffur;
//             }
//             catch (Exception ex)
//             {
//                 return null;
//             }
//             finally
//             {
//                 if (fs != null)
//                 {
//                     //关闭资源
//                     fs.Close();
//                 }
//             }
//         }
//     }
//     #endregion
// }