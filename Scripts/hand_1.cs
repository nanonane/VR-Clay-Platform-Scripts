using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

using System.Threading;
using HoloLensClient;

using Utils;
using RecordStructure;

public class hand_1 : MonoBehaviour
{
    public SwitchModels switchModelsScript;
    public RecNode currNode;
    private GameObject currModel;
    HLClient Client = new HLClient();
    private static int IDCount;
    private static string ModelFileDir = "C:\\Users\\Yingtuww\\Desktop\\output\\Models\\";
    private Serializer sr = new Serializer();

    void Awake()
    {
        // 在 Awake() 方法中尝试获取 SwitchModels 脚本组件
        if (switchModelsScript == null)
        {
            switchModelsScript = GetComponent<SwitchModels>();
            if (switchModelsScript == null)
            {
                switchModelsScript = FindObjectOfType<SwitchModels>();
            }
        }
    }

    void Start()
    {
        // 网络传输初始化
        Client.setUp(); // 初始化
        Client.attemptConnection(); // 连接到服务器

        // 回溯结点初始化
        currNode = new RecNode(0, RecNode.NONE, null, -1); // 傀儡头结点
        IDCount = 1;

        BoneJointInit();
        if (switchModelsScript == null)
        {
            switchModelsScript = GetComponent<SwitchModels>();
            if (switchModelsScript == null)
            {
                switchModelsScript = FindObjectOfType<SwitchModels>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        BoneJointRender();

        // 如果 switchModelsScript 仍然为 null,继续尝试获取
        if (switchModelsScript == null)
        {
            switchModelsScript = GetComponent<SwitchModels>();
            if (switchModelsScript == null)
            {
                switchModelsScript = FindObjectOfType<SwitchModels>();
            }
        }
    }

    List<GameObject> jointObjectL = new List<GameObject>();
    List<GameObject> jointObjectR = new List<GameObject>();

    void BoneJointInit()
    {

        GameObject root1 = new GameObject("Compound Collider");
        GameObject root2 = new GameObject("Compound Collider");
        //root1.AddComponent<MeshCollider>();
        //root1.GetComponent<MeshCollider>().convex = true;
        root1.AddComponent<Rigidbody>();
        root1.GetComponent<Rigidbody>().isKinematic = true;

        //root2.AddComponent<MeshCollider>();
        root2.AddComponent<Rigidbody>();
        //root2.GetComponent<MeshCollider>().convex = true;
        root2.GetComponent<Rigidbody>().isKinematic = true;

        TriggerScript script = root1.AddComponent<TriggerScript>();
        script.hs = this;
        TriggerScript scriptl = root2.AddComponent<TriggerScript>();
        scriptl.hs = this;

        for (int i = 0; i < 5; i++)
        {
            GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj1.name = "JointL";
            obj1.transform.parent = root1.transform;
            obj1.AddComponent<SphereCollider>();
            //Destroy(obj1.GetComponent<Collider>());

            jointObjectL.Add(obj1);

            GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj2.name = "JointR";
            obj2.transform.parent = root2.transform;
            obj2.AddComponent<SphereCollider>();
            //Destroy(obj2.GetComponent<Collider>());

            jointObjectR.Add(obj2);
        }

        //MeshFilter[] meshFilters1 = root1.GetComponentsInChildren<MeshFilter>();
        //MeshFilter[] meshFilters2 = root2.GetComponentsInChildren<MeshFilter>();

        //CombineInstance[] combineL = new CombineInstance[5];
        //CombineInstance[] combineR = new CombineInstance[5];

        //for (int i = 0; i < 5; i++)
        //{
        //    combineL[i].mesh = meshFilters1[i].sharedMesh;
        //    combineL[i].transform = meshFilters1[i].transform.localToWorldMatrix;
        //    combineR[i].mesh = meshFilters2[i].sharedMesh;
        //    combineR[i].transform = meshFilters2[i].transform.localToWorldMatrix;
        //}
        //Mesh mesh1 = new Mesh();
        //Mesh mesh2 = new Mesh();
        //mesh1.CombineMeshes(combineL);
        //mesh2.CombineMeshes(combineR);
        //root1.GetComponent<MeshCollider>().sharedMesh = mesh1;
        //root2.GetComponent<MeshCollider>().sharedMesh = mesh2;




    }
    private int hcount = 0;
    float start_time;

    void Enter()
    {
        hcount++;
        if (hcount == 1)
        {
            isRecordingTrajectory = true;
            start_time = Time.time;
            Debug.Log("Enter");
        }
        
    }
    void Exit()
    {
        hcount--;
        if (hcount == 0)
        {
            isRecordingTrajectory = false;
            WriteHandDataToFile(jointTrajectoriesR, jointTrajectoriesL, ".txt");
            Client.sendMessage(); // 发送手势追踪结果的txt
            Thread receiveThread = new Thread(() => Client.receiveMessage(IDCount)); // 开启接收文件的线程
            receiveThread.Start();
            //Client.receiveMessage(IDCount);
            // 接收到文件，开始渲染模型
            //while (receiveThread.IsAlive) ;
            currNode = new RecNode(IDCount, RecNode.PINCH, "model" + IDCount, currNode.getParentID());
            currModel = switchModelsScript.GetCurrentModel();
            currModel.GetComponent<MeshFilter>().mesh = currNode.getClayModel();
            sr.SerializeNode(currNode, "rec" + IDCount);
            IDCount++;

            jointTrajectoriesR = new Dictionary<int, List<Vector3>>();
            jointTrajectoriesL = new Dictionary<int, List<Vector3>>();
            Debug.Log("Exit");
        }
        
    }


    public class TriggerScript : MonoBehaviour
    {
        public hand_1 hs;

        void OnTriggerEnter(Collider other)
        {
            hs.Enter();
            Debug.Log("is enter");
        }

        void OnTriggerExit(Collider other)
        {
            hs.Exit();
            Debug.Log("is exit");
        }

    }

    MixedRealityPose pose;
    Vector3[] jointR = new Vector3[5];
    Vector3[] jointL = new Vector3[5];
    bool isRecordingTrajectory = false;
    Dictionary<int, List<Vector3>> jointTrajectoriesL = new Dictionary<int, List<Vector3>>();
    Dictionary<int, List<Vector3>> jointTrajectoriesR = new Dictionary<int, List<Vector3>>();
    Color color = Color.white;
    Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
    public string outputFilePath = "C:\\Users\\Yingtuww\\Desktop\\output\\HandPosition"; // 输出文件的路径
    int[] index = { 6, 11, 16, 21, 26 };
   

    void BoneJointRender()
    {
        for (int i = 0; i < 5; i++)
        {
            jointObjectR[i].GetComponent<Renderer>().enabled = false;
            jointObjectL[i].GetComponent<Renderer>().enabled = false;
        }

        for (int i = 0; i < 5; i++)
        {
            if (HandJointUtils.TryGetJointPose((TrackedHandJoint)(index[i]), Handedness.Right, out pose))
            {
                jointR[i] = pose.Position;
                jointObjectR[i].GetComponent<Renderer>().enabled = true;
                if (isRecordingTrajectory)
                {
                    if((Time.time - start_time)%0.05f < 0.01f)
                    {
                        // 记录关节轨迹
                        if (jointTrajectoriesR.ContainsKey(i))
                        {
                            jointTrajectoriesR[i].Add(jointR[i] * 10);
                        }
                        else
                        {
                            jointTrajectoriesR[i] = new List<Vector3> { jointR[i] * 10 };
                        }
                    }
                    
                }
            }

            if (HandJointUtils.TryGetJointPose((TrackedHandJoint)(index[i]), Handedness.Left, out pose))
            {
                jointL[i] = pose.Position;
                jointObjectL[i].GetComponent<Renderer>().enabled = true;
                if (isRecordingTrajectory)
                {
                    // 记录关节轨迹
                    if (jointTrajectoriesL.ContainsKey(i))
                    {
                        jointTrajectoriesL[i].Add(jointL[i] * 10);
                    }
                    else
                    {
                        jointTrajectoriesL[i] = new List<Vector3> { jointL[i] * 10 };
                    }
                }
            }
        }

        color.a = 0f;
        for (int i = 0; i < 5; i++)
        {
            jointObjectR[i].transform.position = jointR[i];
            jointObjectR[i].GetComponent<Renderer>().material.color = color;
            jointObjectR[i].transform.localScale = scale;

            jointObjectL[i].transform.position = jointL[i];
            jointObjectL[i].GetComponent<Renderer>().material.color = color;
            jointObjectL[i].transform.localScale = scale;
        }


    }


    public GameObject Opsubject;
    float angle;
    Vector3 axis;
    void WriteHandDataToFile(Dictionary<int, List<Vector3>> jointTrajectoriesR, Dictionary<int, List<Vector3>> jointTrajectoriesL, string date)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath + date, false))
        {
            float angle;
            Vector3 axis;
            //writer.WriteLine("Model Name:");
            //writer.WriteLine(switchModelsScript.GetCurrentModelName());
            writer.WriteLine("Model Positions:");
            writer.WriteLine(FormatVector(Opsubject.transform.position * 10));
            Opsubject.transform.rotation.ToAngleAxis(out angle, out axis);
            writer.WriteLine("rotation:");
            writer.WriteLine(FormatVector(axis));
            writer.WriteLine("rotdeg:");
            writer.WriteLine(angle);
            writer.WriteLine("Light Hand Positions:");
            foreach (var kvp in jointTrajectoriesL)
            {
                int jointIndex = kvp.Key;
                List<Vector3> trajectory = kvp.Value;

                writer.WriteLine("Joint " + (jointIndex + 1) + " trajectory:");

                for (int i = 0; i < trajectory.Count; i++)
                {
                    writer.WriteLine("Position " + (i + 1) + ": " + FormatVector(trajectory[i]));
                }

                writer.WriteLine();
            }

            writer.WriteLine("Right Hand Positions:");
            foreach (var kvp in jointTrajectoriesR)
            {
                int jointIndex = kvp.Key;
                List<Vector3> trajectory = kvp.Value;

                writer.WriteLine("Joint " + (jointIndex + 1) + " trajectory:");

                for (int i = 0; i < trajectory.Count; i++)
                {
                    //writer.WriteLine("Position " + (i + 1) + ": " + FormatVector(trajectory[i]));
                    writer.WriteLine(FormatVector(trajectory[i]));
                }

                writer.WriteLine();
            }
        }
    }

    string FormatVector(Vector3 vector)
    {
        return string.Format("({0:F5}, {1:F5}, {2:F5})", vector.x, vector.y, vector.z);
    }

}


