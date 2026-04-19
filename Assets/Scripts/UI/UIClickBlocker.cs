using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickBlocker : MonoBehaviour, IPointerClickHandler
{
    public System.Action OnClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke();
    }
}
