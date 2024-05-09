using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Utils;

namespace RecordStructure
{
    [Serializable()]
    public class RecNode
    {
        public const int PINCH = 5050; // 捏
        public const int PRESS = 5051; // 压
        public const int SMOOTH = 5052; // 抹
        public const int NONE = 5055; // 什么都没有

        // private static int IDCount = 0; // identification for the current record. 0-indexed.
        private int ID;
        private int operationType; // 捏、压、抹
        private DateTime timestamp; // 操作时间
        private string modelName; // 模型
        private int depth; // 当前节点到根节点的深度，根节点深度为0
        private int parentID; // 指向前一次记录。后续考虑更改为Hash String，作为记录文件名

        public RecNode(int ID, int operationType, string modelName, int parentID)
        {
            this.ID = ID;
            this.operationType = operationType;
            this.timestamp = DateTime.Now;
            this.modelName = modelName;
            this.parentID = parentID;
            if (parentID == -1)
                this.depth = 0;
            else
                this.depth = getParent().depth + 1;
        }

        /* use the given id maintained by the class to initialize the node */
        // public RecNode(int operationType, RecNode parent)
        // {
        //     this.ID = IDCount;
        //     this.operationType = operationType;
        //     this.timestamp = DateTime.Now;
        //     this.modelName = "Clay" + IDCount;
        //     this.parent = parent;
        //     if (parent == null)
        //         this.depth = 0;
        //     else
        //         this.depth = parent.depth + 1;
        //     IDCount++;
        // }

        public Mesh getClayModel()
        {
            MeshImporter mi = new MeshImporter();
            Mesh mesh = mi.importModel(modelName);
            return mesh;
        }

        public int getDepth()
        {
            return depth;
        }

        public int getOperationType()
        {
            return operationType;
        }

        public int getParentID()
        {
            return parentID;
        }

        /* Return the parent node of this one. */
        public RecNode getParent()
        {
            if (parentID == -1) return null;
            Serializer sr = new Serializer();
            return sr.DeSerializeNode("rec" + parentID); 
        }
    }

    [Serializable()]
    public class Branches
    {
        private Dictionary<int, RecNode> branches; // 从分支编号到节点的映射
        private int currentBranch; // 当前分支的编号

        public Branches()
        {
            this.currentBranch = 0;
            this.branches = new Dictionary<int, RecNode>();
        }

        public Dictionary<int, RecNode> getBranches()
        {
            return branches;
        }

        public void addBranch(int branchID, RecNode node)
        {
            branches.Add(branchID, node);
        }
    }

    public class Serializer
    {
        private static string RecDir = "C:\\Users\\Yingtuww\\Desktop\\output\\Serialization\\";
        public void SerializeNode(RecNode node, string filename)
        {
            Stream s = File.Open(RecDir + filename, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, node);
            s.Close();
        }
        public RecNode DeSerializeNode(string filename)
        {
            Stream s = File.Open(RecDir + filename, FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            RecNode node = (RecNode)b.Deserialize(s);
            s.Close();
            return node;
        }
    }
}
