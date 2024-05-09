using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RecordStructure;
using TMPro;

public class traceRecords : MonoBehaviour
{
    private const int BUTTON_NUM = 5;
    /* 控制翻页 */
    private const int PREV_PAGE = -1;
    private const int NEXT_PAGE = 1;
    private Serializer sr = new Serializer();
    public GameObject previewModel;
    public GameObject[] recordButtons;
    public GameObject[] recordText;

    /* 记录当前分支的叶节点 */
    private RecNode lastNode;
    /* for iteration */
    private RecNode currNode;
    /* the offset between button number and record node number */
    private int offset;
    /* The current page which user is looking at. 0-indexed. */
    private int currPage;
    /* The maximum page number the records can take up. Still 0-indexed. */
    private int maxPage;
    private List<RecNode> pageFirstNode;


    /* These initializations are for testing. 
     * After implementing the Server part, we should be able to directly generate and read meshes from files.
    */
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

    // Start is called before the first frame update
    void Start()
    {
        // Initializations for testing.
        node0 = sr.DeSerializeNode("rec0");
        node1 = sr.DeSerializeNode("rec1");
        node2 = sr.DeSerializeNode("rec2");
        node3 = sr.DeSerializeNode("rec3");
        node4 = sr.DeSerializeNode("rec4");
        node5 = sr.DeSerializeNode("rec5");
        node6 = sr.DeSerializeNode("rec6");
        node7 = sr.DeSerializeNode("rec7");
        node8 = sr.DeSerializeNode("rec8");
        node9 = sr.DeSerializeNode("rec9");
        node10 = sr.DeSerializeNode("rec10");
        node11 = sr.DeSerializeNode("rec11");

        lastNode = node11;

        // delete this line when combining this part into the platform
        menuSetUp();
    }

    public void menuSetUp()
    {
        currNode = lastNode;
        currPage = 0;
        maxPage = lastNode.getDepth() / BUTTON_NUM;
        pageFirstNode = new List<RecNode>();
        pageFirstNode.Add(lastNode);
        gameObject.SetActive(true);
        previewModel.SetActive(true);
        showMenuButtons(lastNode.getDepth() + 1);
    }

    /* Show the record buttons on the menu. */
    public void showMenuButtons(int nodeNum)
    {
        int activeButtonNum; // 需要显示的按钮数量
        offset = nodeNum - BUTTON_NUM;
        if (offset > 0) { activeButtonNum = 5; }
        else { activeButtonNum = nodeNum; }
        for (int i = 0; i < activeButtonNum; i++)
        {
            recordButtons[i].SetActive(true);
            recordText[i].GetComponent<TextMeshPro>().text = getOp(currNode.getOperationType());
            currNode = currNode.getParent();
        }
        // 当前页显示完后，currNode指向下一页的第一项
    }

    public void switchPage(int pageChange)
    {
        if (pageChange == PREV_PAGE)
        {
            if (currPage == 0) return;
            currPage -= 1;
            currNode = pageFirstNode[currPage];
            showMenuButtons(currNode.getDepth() + 1);
            return;
        }
        if (pageChange == NEXT_PAGE)
        {
            if (currPage == maxPage) return;
            for (int i = 0; i < BUTTON_NUM; i++) recordButtons[i].SetActive(false);
            currPage += 1;
            if (!pageFirstNode.Contains(currNode)) pageFirstNode.Add(currNode);
            showMenuButtons(currNode.getDepth() + 1);
            return;
        }
    }

    public void switchMesh(int buttonID)
    {
        RecNode showMesh = pageFirstNode[currPage];
        for (int i = 0; i < buttonID; i++)
        {
            showMesh = showMesh.getParent();
        }
        previewModel.GetComponent<MeshFilter>().mesh = showMesh.getClayModel();
    }

    private string getOp(int operation)
    {
        switch (operation)
        {
            case (RecNode.PINCH):
                return "Pinch";
            case (RecNode.PRESS):
                return "Press";
            case (RecNode.SMOOTH):
                return "Smooth";
            default:
                return null;
        }
    }
}
