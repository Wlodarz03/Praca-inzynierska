using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text infoTitle;
    [SerializeField] private Text infoText;
    [SerializeField] private Text messageTitle;
    [SerializeField] private Text messageText;

    public void OnButtonHover(string title, string text)
    {
        messagePanel.SetActive(true);
        messageTitle.text = title;
        messageText.text = text;
    }

    public void OnButtonExit()
    {
        messagePanel.SetActive(false);
    }

    public void OnPictureHover(string title, string text)
    {
        infoPanel.SetActive(true);
        infoTitle.text = title;
        infoText.text = text;
    }

    public void OnPictureExit()
    {
        infoPanel.SetActive(false);
    }
}
