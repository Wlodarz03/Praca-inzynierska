using UnityEngine;

public class HoverManager : MonoBehaviour
{
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Animator infoAnimator;

    private int hoverCount = 0;
    private int pictureHoverCount = 0;

    private bool hamsterFlag = false;

    public void SetHamsterFlag(bool value)
    {
        hamsterFlag = value;
    }

    public bool GetHamsterFlag()
    {
        return hamsterFlag;
    }

    public void OnAnyButtonHoverEnter()
    {
        hoverCount++;
        if (hoverCount >= 1)
        {
            buttonAnimator.SetBool("IsHover", true);
        }
    }

    public void OnAnyButtonHoverExit()
    {
        hoverCount--;
        if (hoverCount <= 0)
        {
            hoverCount = 0;
            buttonAnimator.SetBool("IsHover", false);
        }
    }

    public void OnPictureHoverEnter()
    {
        pictureHoverCount++;
        if (pictureHoverCount >= 1)
        {
            infoAnimator.SetBool("IsHover", true);
        }
    }

    public void OnPictureHoverExit()
    {
        pictureHoverCount--;
        if (pictureHoverCount <= 0)
        {
            pictureHoverCount = 0;
            infoAnimator.SetBool("IsHover", false);
        }
    }
    

}