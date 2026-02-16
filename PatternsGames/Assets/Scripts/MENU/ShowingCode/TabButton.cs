using UnityEngine;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    private int index;
    private float alfa = 0.5019608f;
    private CodePanelManager manager;

    public float GetAlfa()
    {
        return alfa;
    }

    public void Init(string fileName, int index, CodePanelManager manager)
    {
        label.text = fileName;
        this.index = index;
        this.manager = manager;
    }

    public void OnClick()
    {
        Debug.Log("Tab clicked: " + index);
        manager.ShowFile(index);
    }
}
