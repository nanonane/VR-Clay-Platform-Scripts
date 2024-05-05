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

public class hand_1 : MonoBehaviour
{
    HLClient Client = new HLClient();
    // Start is called before the first frame update
    void Start()
    {
        Client.setUp(); // 初始化
        Client.attemptConnection(); // 连接到服务器
        Thread receiveThread = new Thread(Client.receiveMessage); // 开启接收文件的线程
        receiveThread.Start();
        BoneJointInit();
    }

    // Update is called once per frame
    void Update()
    {
        BoneJointRender();
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

    void Enter()
    {
        hcount++;
        if (hcount == 1)
        {
            isRecordingTrajectory = true;
            Client.sendMessage(); // 发送手势追踪结果的txt
            Debug.Log("Enter");
        }
    }
    void Exit()
    {
        hcount--;
        if (hcount == 0)
        {
            isRecordingTrajectory = false;
            WriteHandDataToFile(jointTrajectoriesR, jointTrajectoriesL, DateTime.Now.ToString("HH_mm_ss_ff") + ".txt");
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
    public bool isRecordingTrajectory = false;
    Dictionary<int, List<Vector3>> jointTrajectoriesL = new Dictionary<int, List<Vector3>>();
    Dictionary<int, List<Vector3>> jointTrajectoriesR = new Dictionary<int, List<Vector3>>();
    Color color = Color.white;
    Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
    public string outputFilePath = "E:\\train_2023\\output\\"; // 输出文件的路径
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
                    // 记录关节轨迹
                    if (jointTrajectoriesR.ContainsKey(i))
                    {
                        jointTrajectoriesR[i].Add(jointR[i]);
                    }
                    else
                    {
                        jointTrajectoriesR[i] = new List<Vector3> { jointR[i] };
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
                        jointTrajectoriesL[i].Add(jointL[i]);
                    }
                    else
                    {
                        jointTrajectoriesL[i] = new List<Vector3> { jointL[i] };
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

    void WriteHandDataToFile(Dictionary<int, List<Vector3>> jointTrajectoriesR, Dictionary<int, List<Vector3>> jointTrajectoriesL, string date)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath + date, false))
        {
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
                    writer.WriteLine(FormatVector(trajectory[i]) + ',');
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


