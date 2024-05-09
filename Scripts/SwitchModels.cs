using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SwitchModels : MonoBehaviour
{
    public GameObject model1; // ���ǵ�һ��ģ��
    public GameObject model2; // ���ǵڶ���ģ��
    public GameObject model3; // ���ǵ�����ģ��
    public Interactable myButton; // �������ǵİ�ť

    public int currentModel = 1; // ��ǰ��ʾ��ģ�͵ı��

    void Start()
    {
        // �ڿ�ʼʱ����Ӱ�ť�ĵ���¼�
        myButton.OnClick.AddListener(Switch);
    }

    public void Switch()
    {
        // ���ص�ǰ��ʾ��ģ��
        GetCurrentModel().SetActive(false);

        // ���µ�ǰ��ʾ��ģ�͵ı��
        currentModel = currentModel % 3 + 1;

        // ��ʾ�µ�ģ��
        GetCurrentModel().SetActive(true);
    }

    public GameObject GetCurrentModel()
    {
        // ���ݵ�ǰ��ʾ��ģ�͵ı�ţ����ض�Ӧ��ģ��
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
