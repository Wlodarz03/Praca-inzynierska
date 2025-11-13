using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class CommandHistoryVisualizer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform startArrow;
    [SerializeField] private RectTransform currentArrow;
    [SerializeField] private GameObject[] slots;

    [Header("Animation Settings")]
    [SerializeField] private float arrowMoveSpeed = 8f;

    private Vector3 targetStartPos;
    private Vector3 targetCurrentPos;
    private Quaternion targetStartRot;
    private Quaternion targetCurrentRot;

    private void Awake()
    {
        targetStartPos = startArrow.localPosition;
        targetCurrentPos = currentArrow.localPosition;
        targetStartRot = startArrow.localRotation;
        targetCurrentRot = currentArrow.localRotation;
    }

    private void Update()
    {
        // płynne przejście pozycji i rotacji
        startArrow.localPosition = Vector3.Lerp(startArrow.localPosition, targetStartPos, Time.deltaTime * arrowMoveSpeed);
        currentArrow.localPosition = Vector3.Lerp(currentArrow.localPosition, targetCurrentPos, Time.deltaTime * arrowMoveSpeed);

        startArrow.localRotation = Quaternion.Lerp(startArrow.localRotation, targetStartRot, Time.deltaTime * arrowMoveSpeed);
        currentArrow.localRotation = Quaternion.Lerp(currentArrow.localRotation, targetCurrentRot, Time.deltaTime * arrowMoveSpeed);
    }

    public void UpdateText(int ind, string text)
    {
        if (slots == null || ind < 0 || ind >= slots.Length)
            return;

        var tmp = slots[ind].transform.Find("Komenda")?.GetComponent<TextMeshProUGUI>();

        if (tmp == null) return;

        tmp.text = text;
    }

    public void ClearBuffer()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            UpdateText(i, "");
        }
    }

    public void UpdateCurrentArrow(int ind, int startIndex, int maxSize)
    {
        if (ind < 0 || ind >= maxSize) return;
        var slotRect = slots[ind].GetComponent<RectTransform>();
        if (slotRect == null) return;

        float yOffset = (ind < 10) ? 190f : -190f;
        float xOffset = 0f;

        if (ind == startIndex || ind == -1)
            xOffset = 37.5f;

        targetCurrentPos = slotRect.localPosition + new Vector3(xOffset, yOffset, 0f);
        targetCurrentRot = (ind < 10) ? Quaternion.identity : Quaternion.Euler(0f, 0f, 180f);

    }

    public void UpdateStartArrow(int ind, int currentIndex, int maxSize)
    {
        if (ind < 0 || ind >= maxSize) return;
        var slotRect = slots[ind].GetComponent<RectTransform>();
        if (slotRect == null) return;

        float yOffset = (ind < 10) ? 190f : -190f;
        float xOffset = 0f;

        if (ind == currentIndex || currentIndex == -1)
            xOffset = -37.5f;

        targetStartPos = slotRect.localPosition + new Vector3(xOffset, yOffset, 0f);
        targetStartRot = (ind < 10) ? Quaternion.identity : Quaternion.Euler(0f, 0f, 180f);
    }

    public void TextColorChange(int ind, Color color)
    {
        var tmp = slots[ind].transform.Find("Komenda")?.GetComponent<TextMeshProUGUI>();
        tmp.color = color;
    }

    public void BackgroundColorChange(int ind, Color color)
    {
        var img = slots[ind].transform.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }

    public void ResetVisualizer()
    {
        ClearBuffer();

        for (int i = 0; i < slots.Length; i++)
        {
            TextColorChange(i, Color.black);
            BackgroundColorChange(i, new Color(0.7f, 0.7f, 0.7f, 0.6f));
        }

        if (slots.Length > 0)
        {
            startArrow.localRotation = Quaternion.identity;
            currentArrow.localRotation = Quaternion.identity;

            startArrow.localPosition = slots[0].GetComponent<RectTransform>().localPosition + new Vector3(-37.5f, 190f, 0f);
            currentArrow.localPosition = slots[0].GetComponent<RectTransform>().localPosition + new Vector3(37.5f, 190f, 0f);

            targetStartPos = startArrow.localPosition;
            targetCurrentPos = currentArrow.localPosition;
            targetStartRot = startArrow.localRotation;
            targetCurrentRot = currentArrow.localRotation;
        }
    }

}
