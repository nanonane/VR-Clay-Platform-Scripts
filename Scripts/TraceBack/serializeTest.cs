using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RecordStructure;

public class serializeTest : MonoBehaviour
{
    RecNode node11;
    RecNode node10;
    RecNode node9;
    RecNode node8;
    RecNode node7;
    RecNode node6;
    RecNode node5;
    RecNode node4;
    RecNode node3;
    RecNode node2;
    RecNode node1;
    RecNode node0;
    Serializer sr = new Serializer();

    // Start is called before the first frame update
    void Start()
    {
        node0 = new RecNode(0, RecNode.SMOOTH, "01", null);
        node1 = new RecNode(1, RecNode.PINCH, "03", node0);
        node2 = new RecNode(2, RecNode.PRESS, "05", node1);
        node3 = new RecNode(3, RecNode.PRESS, "07", node2);
        node4 = new RecNode(4, RecNode.SMOOTH, "10", node3);
        node5 = new RecNode(5, RecNode.PINCH, "11", node4);
        node6 = new RecNode(6, RecNode.SMOOTH, "13", node5);
        node7 = new RecNode(7, RecNode.PINCH, "15", node6);
        node8 = new RecNode(8, RecNode.SMOOTH, "17", node7);
        node9 = new RecNode(9, RecNode.PRESS, "20", node8);
        node10 = new RecNode(10, RecNode.PRESS, "21", node9);
        node11 = new RecNode(11, RecNode.PINCH, "23", node10);
        sr.SerializeNode(node11, "node11");
        sr.SerializeNode(node10, "node10");
        sr.SerializeNode(node9, "node9");
        sr.SerializeNode(node8, "node8");
        sr.SerializeNode(node7, "node7");
        sr.SerializeNode(node6, "node6");
        sr.SerializeNode(node5, "node5");
        sr.SerializeNode(node4, "node4");
        sr.SerializeNode(node3, "node3");
        sr.SerializeNode(node2, "node2");
        sr.SerializeNode(node1, "node1");
        sr.SerializeNode(node0, "node0");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
