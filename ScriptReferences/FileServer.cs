// // using LitJson
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;
// using UnityEngine;
 
// //Json数据
// [Serializable]
// public class JsonData
// {
//     public string operationType; //服务器事件类型（选择服务器要执行的方法）
//     public string clientIP;      //客户端本地ip
//     public string flieName;      //文件名称(文件名称)
//     public byte[] fileData;      //文件byte[]数据
// }
 
// public class FileServer : MonoBehaviour
// {
//     private string myip = "127.0.0.1";
//     private int myPort = 5753;   //端口   
//     string saveFilePath;  //保存地址
//     JsonData jsonData;
 
//     static Socket serverSocket;
//     Thread myThread;
//     Dictionary<string, Thread> threadDic = new Dictionary<string, Thread>();//存储线程，程序结束后关闭线程
//     private void Start()
//     {
//         // myip = ConfigFile.LoadString("Server");
//         // myPort = int.Parse(ConfigFile.LoadString("port"));
//         saveFilePath = Application.streamingAssetsPath + "/"; //文件保存路径
 
 
//         //服务器IP地址  ，127.0.0.1 为本机IP地址
//         IPAddress ip = IPAddress.Parse(myip);
//         Debug.Log(ip.ToString());
//         serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
 
//         IPEndPoint iPEndPoint = new IPEndPoint(ip, myPort);
//         serverSocket.Bind(iPEndPoint);  //绑定IP地址：端口  
//         serverSocket.Listen(10);        //最多10个连接请求  
//                                         //Console.WriteLine("creat service {0} success",
//                                         //    serverSocket.LocalEndPoint.ToString());
 
//         myThread = new Thread(ListenClientConnect);
//         myThread.Start();
//         Debug.Log("服务器启动...........");
 
//     }
 
//     // 监听客户端是否连接  
//     private void ListenClientConnect()
//     {
//         while (true)
//         {
//             Socket clientSocket = serverSocket.Accept(); //1.创建一个Socket 接收客户端发来的连接请求 没有时堵塞
//             string clientIp = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
 
//             Debug.Log("客户端连接成功：" + clientIp);
//             clientSocket.Send(Encoding.UTF8.GetBytes("连接成功")); //2.向客户端发送 连接成功 消息
//             Thread receiveThread = new Thread(ReceiveMessage); //3.为已经连接的客户端创建一个线程 此线程用来处理客户端发送的消息
//             receiveThread.Start(clientSocket); //4.开启线程
 
//             //将已经连接的客户端添加到字典中
//             if (!threadDic.ContainsKey(clientIp))
//             {
//                 threadDic.Add(clientIp, receiveThread);
//             }
//         }
//     }
 
 
//     private byte[] result = new byte[1024]; //1.存入的byte值 最大数量1024
//     private byte[] subpackageData; //1.分包数据存入信息
 
//     int dataLength;     //要接收信息总的长度
//     bool isEndData = false; //是否接收到信息总长度
//     //开启线程接收数据 （将Socket作为值传入）
//     private void ReceiveMessage(object clientSocket)
//     {
//         Socket myClientSocket = (Socket)clientSocket; //2.转换传入的客户端Socket
//         while (true)
//         {
//             try
//             {
//                 result = new byte[1024]; //接收前要清空 否则写入时会保留后面的数值
//                 //接收数据  
//                 int receiveNumber = myClientSocket.Receive(result); //3.将客户端得到的byte值写入
 
//                 //没有接收信息总长度
//                 if (!isEndData)
//                 {
//                     //接收
//                     dataLength = int.Parse(Encoding.UTF8.GetString(result));
//                     subpackageData = new byte[0]; //初始化缓冲区
//                     Debug.Log("1.接收到信息的总长度： " + dataLength);
//                     isEndData = true;
//                 }
//                 else
//                 {
//                     //有数据 并且 接收到信息长度后执行
//                     if (receiveNumber > 0 && isEndData)
//                     {
//                         //1.整合数据
//                         byte[] newResult = new byte[(subpackageData.Length + receiveNumber)];
//                         Array.Copy(subpackageData, 0, newResult, 0, subpackageData.Length); //将上一次分包数据赋值给容器
//                         Array.Copy(result, 0, newResult, subpackageData.Length, receiveNumber); //将当前收到的数据赋值给容器
 
//                         subpackageData = newResult;
//                         Debug.Log("整合后数据长度： " + subpackageData.Length);
 
 
//                         //整合的数据超出 或等于数据
//                         if (subpackageData.Length >= dataLength)
//                         {
//                             isEndData = false; //设置没有接收到信息总长度
//                             //数据解析
//                             string data = Encoding.UTF8.GetString(subpackageData);
//                             Debug.Log(data);
//                             DisposeData(data);
 
//                             myClientSocket.Send(Encoding.UTF8.GetBytes("文件转换成功" + DateTime.Now));
//                         }
//                     }
//                     else
//                     {
//                         Debug.Log("client： " + ((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString() + "断开连接");
//                         threadDic[((IPEndPoint)myClientSocket.RemoteEndPoint).Address.ToString()].Abort(); //清除线程
//                     }
//                 }
 
//             }
//             catch (Exception ex)
//             {
//                 //myClientSocket.Shutdown(SocketShutdown.Both); //出现错误 关闭Socket
//                 Debug.Log(" 错误信息" + ex); //打印错误信息
//                 break;
//             }
//         }
//     }
 
//     #region 数据处理
//     private void DisposeData(string data)
//     {
//         //UTF8Encoding m_utf8 = new UTF8Encoding(false); //这是不带有BOM的UTF-8 使用带有BOM的UTF-8转换Json容易失败
//         jsonData = JsonMapper.ToObject<JsonData>(data);
 
//         string savePath = saveFilePath + jsonData.flieName;
//         Debug.Log(savePath);
//         //上传附件 判断是否存在
//         if (File.Exists(savePath))
//         {
//             //文件存在 重命名新文件
//             savePath = saveFilePath + "(Colon)"+jsonData.flieName;
//         }
 
//         Bytes2File(jsonData.fileData, savePath); //将byte数据转换为文件
//     }
//     #endregion
 
//     #region 将byte数组转换为文件并保存到指定地址
//     /// <summary>
//     /// 将byte数组转换为文件并保存到指定地址
//     /// </summary>
//     /// <param name="buff">byte数组</param>
//     /// <param name="savepath">保存地址</param>
//     public static void Bytes2File(byte[] buff, string savepath)
//     {
//         if (File.Exists(savepath))
//         {
//             File.Delete(savepath);
//         }
 
//         FileStream fs = new FileStream(savepath, FileMode.CreateNew);
//         BinaryWriter bw = new BinaryWriter(fs);
//         bw.Write(buff, 0, buff.Length);
//         bw.Close();
//         fs.Close();
//     }
//     #endregion
 
//     void OnApplicationQuit()
//     {
//         //结束线程必须关闭 否则下次开启会出现错误 （如果出现的话 只能重启unity了）
//         myThread.Abort();
 
//         //关闭开启的线程
//         foreach (string item in threadDic.Keys)
//         {
//             Debug.Log(item);//de.Key对应于key/value键值对key
//             //item.Value.GetType()
//             threadDic[item].Abort();
//         }
//     }
 
// }