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
    public RecNode lastNode;
    /* for iteration */
    private RecNode currNode;
    /* the offset between button number and record node number */
    private int offset;
    /* The current page which user is looking at. 0-indexed. */
    private int currPage;
    /* The maximum page number the records can take up. Still 0-indexed. */
    private int maxPage;
    private List<RecNode> pageFirstNode;

    // Start is called before the first frame update
    void Start()
    {
        lastNode = sr.DeSerializeNode("rec11");
        menuSetUp();
    }

    void OnEnable()
    {
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
        showMenuButtons(lastNode.getDepth());
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
            showMenuButtons(currNode.getDepth());
            return;
        }
        if (pageChange == NEXT_PAGE)
        {
            if (currPage == maxPage) return;
            for (int i = 0; i < BUTTON_NUM; i++) recordButtons[i].SetActive(false);
            currPage += 1;
            if (!pageFirstNode.Contains(currNode)) pageFirstNode.Add(currNode);
            showMenuButtons(currNode.getDepth());
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
