using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private SlotPosition slotPosition;
    [SerializeField] private GameObject button;
    [SerializeField] private bool canHover = false;
    private PlayCardData cardData;

    private UICard currentCardUI;

    public bool IsOccupied => currentCardUI != null;

    public void SetCard(PlayCardData card)
    {
        if (IsOccupied) return;

        cardData = card;
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

    public void ReturnCardToHand()
    {
        if(canHover && cardData != null)
        {
            GameManager.Instance.Players[PlayerSide.Player].PlayerCards.UnsetCard(cardData);
            ClearSlot();
            cardData = null;
            this.button.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!canHover) return;

        if (cardData != null)
        {
            this.button.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!canHover) return;
        this.button.SetActive(false);
    }
}
