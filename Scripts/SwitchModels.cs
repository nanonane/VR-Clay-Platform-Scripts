using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SwitchModels : MonoBehaviour
{
    public GameObject model1; // 这是第一个模型
    public GameObject model2; // 这是第二个模型
    public GameObject model3; // 这是第三个模型
    public Interactable myButton; // 这是我们的按钮

    public int currentModel = 1; // 当前显示的模型的编号

    void Start()
    {
        // 在开始时，添加按钮的点击事件
        myButton.OnClick.AddListener(Switch);
    }

    public void Switch()
    {
        // 隐藏当前显示的模型
        GetCurrentModel().SetActive(false);

        // 更新当前显示的模型的编号
        currentModel = currentModel % 3 + 1;

        // 显示新的模型
        GetCurrentModel().SetActive(true);
    }

    public GameObject GetCurrentModel()
    {
        // 根据当前显示的模型的编号，返回对应的模型
        switch (currentModel)
        {
            case 1: return model1;
            case 2: return model2;
            case 3: return model3;
            default: return null;
        }
    }
    public string GetCurrentModelName()
    {
        switch (currentModel)
        {
            case 1: return "Model_1";
            case 2: return "Model_2";
            case 3: return "Model_3";
            default: return "Unknown";
        }
    }

}
