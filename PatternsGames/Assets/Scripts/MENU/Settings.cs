using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private Image icon;

    public void OnClick(){
        if (buttons.activeSelf)
        {
            buttons.SetActive(false);
        }
        else
        {
            buttons.SetActive(true);
        }
    }

    public void OnHoverStart()
    {
        icon.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        icon.transform.rotation = Quaternion.Euler(0, 0, -60);
    }

    public void OnHoverEnd()
    {
        icon.transform.localScale = Vector3.one;
        icon.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
