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
        node0 = new RecNode(0, RecNode.SMOOTH, "01", -1);
        sr.SerializeNode(node0, "rec0");
        node1 = new RecNode(1, RecNode.PINCH, "03", 0);
        sr.SerializeNode(node1, "rec1");
        node2 = new RecNode(2, RecNode.PRESS, "05", 1);
        sr.SerializeNode(node2, "rec2");
        node3 = new RecNode(3, RecNode.PRESS, "07", 2);
        sr.SerializeNode(node3, "rec3");
        node4 = new RecNode(4, RecNode.SMOOTH, "10", 3);
        sr.SerializeNode(node4, "rec4");
        node5 = new RecNode(5, RecNode.PINCH, "11", 4);
        sr.SerializeNode(node5, "rec5");
        node6 = new RecNode(6, RecNode.SMOOTH, "13", 5);
        sr.SerializeNode(node6, "rec6");
        node7 = new RecNode(7, RecNode.PINCH, "15", 6);
        sr.SerializeNode(node7, "rec7");
        node8 = new RecNode(8, RecNode.SMOOTH, "17", 7);
        sr.SerializeNode(node8, "rec8");
        node9 = new RecNode(9, RecNode.PRESS, "20", 8);
        sr.SerializeNode(node9, "rec9");
        node10 = new RecNode(10, RecNode.PRESS, "21", 9);
        sr.SerializeNode(node10, "rec10");
        node11 = new RecNode(11, RecNode.PINCH, "23", 10);
        sr.SerializeNode(node11, "rec11");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
