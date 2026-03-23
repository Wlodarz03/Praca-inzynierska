using UnityEngine;
using UnityEngine.EventSystems;

public class PictureHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private HoverManager hoverManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverManager.OnPictureHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverManager.OnPictureHoverExit();
    }
}