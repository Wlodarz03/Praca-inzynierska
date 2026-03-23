using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private HoverManager hoverManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverManager.OnAnyButtonHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverManager.OnAnyButtonHoverExit();
    }
}