using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverImage;
    [SerializeField] private bool disableHoverOnResolve = false;
    
    public void Start()
    {
        hoverImage.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disableHoverOnResolve && GameManager.Instance.GetTurnState() == TurnState.ResolvingState)
        {
            return;
        }
        hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }
}
