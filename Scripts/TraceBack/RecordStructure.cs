using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordStructure
{
    public class RecNode
    {
        public const int PINCH = 5050; // 捏
        public const int PRESS = 5051; // 压
        public const int SMOOTH = 5052; // 抹

        private int ID; // identificaiton for the current record. 0-indexed.
        private int operationType; // 捏、压、抹
        private DateTime timestamp; // 操作时间
        private Mesh clayModel; // 模型
        private int depth; // 当前节点到根节点的深度，根节点深度为0
        private RecNode parent; // 指向前一次记录。后续考虑更改为Hash String，作为记录文件名

        public RecNode(int ID, int operationType, Mesh clayModel, RecNode parent)
        {
            this.ID = ID;
            this.operationType = operationType;
            this.timestamp = DateTime.Now;
            this.clayModel = clayModel;
            this.parent = parent;
            if (parent == null) { this.depth = 0; }
            else { this.depth = parent.depth + 1; }
        }

        public Mesh getClayModel()
        {
            return clayModel;
        }

        public int getDepth()
        {
            return depth;
        }

        public int getOperationType()
        {
            return operationType;
        }

        /* Return the parent node of this one. */
        // parent字段更改为String后需要相应地更改这个方法
        public RecNode getParent()
        {
            return parent;
        }
    }

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
}
