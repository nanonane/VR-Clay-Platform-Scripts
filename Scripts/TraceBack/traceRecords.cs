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
    private List<RecNode> pageFirstNode = new List<RecNode>();


    /* These initializations are for testing. 
     * After implementing the Server part, we should be able to directly generate and read meshes from files.
    */
    public Mesh mesh12;
    public Mesh mesh11;
    public Mesh mesh10;
    public Mesh mesh9;
    public Mesh mesh8;
    public Mesh mesh7;
    public Mesh mesh6;
    public Mesh mesh5;
    public Mesh mesh4;
    public Mesh mesh3;
    public Mesh mesh2;
    public Mesh mesh1;

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
        node0 = new RecNode(0, RecNode.SMOOTH, mesh1, null);
        node1 = new RecNode(1, RecNode.PINCH, mesh2, node0);
        node2 = new RecNode(2, RecNode.PRESS, mesh3, node1);
        node3 = new RecNode(3, RecNode.PRESS, mesh4, node2);
        node4 = new RecNode(4, RecNode.SMOOTH, mesh5, node3);
        node5 = new RecNode(5, RecNode.PINCH, mesh6, node4);
        node6 = new RecNode(6, RecNode.SMOOTH, mesh7, node5);
        node7 = new RecNode(7, RecNode.PINCH, mesh8, node6);
        node8 = new RecNode(8, RecNode.SMOOTH, mesh9, node7);
        node9 = new RecNode(9, RecNode.PRESS, mesh10, node8);
        node10 = new RecNode(10, RecNode.PRESS, mesh11, node9);
        node11 = new RecNode(11, RecNode.PINCH, mesh12, node10);

        lastNode = node11;

        // delete this line when combining this part into the platform
        menuSetUp();
    }

    public void menuSetUp()
    {
        currNode = lastNode;
        currPage = 0;
        maxPage = lastNode.getDepth() / BUTTON_NUM;
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
            recordText[i].GetComponent<TextMeshProUGUI>().text = getOp(currNode.getOperationType());
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
