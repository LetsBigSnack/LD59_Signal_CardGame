using Data;
using UnityEngine;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private SlotPosition slotPosition;

    private UICard currentCardUI;

    public bool IsOccupied => currentCardUI != null;

    public void SetCard(PlayCardData card)
    {
        if (IsOccupied) return;

        GameObject cardObj = UICardManager.Instance.CreateSlotCardUI(card, cardParent);
        currentCardUI = cardObj.GetComponent<UICard>();
    }

    public SlotPosition GetSlotPosition()
    {
        return slotPosition;
    }

    public void ClearSlot()
    {
        if (currentCardUI != null)
        {
            Destroy(currentCardUI.gameObject);
            currentCardUI = null;
        }
    }
}
