using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private SlotPosition slotPosition;
    [SerializeField] private PlayerSide playerSide;
    [SerializeField] private GameObject button;
    [SerializeField] private bool canHover = false;
    
    //private PlayCardData cardData;

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

    public void ReturnCardToHand()
    {
        PlayCardData cardData = GameManager.Instance.GetCardOnSlot(playerSide, slotPosition);
        if(canHover && cardData != null && GameManager.Instance.UnsetCardToSlot(PlayerSide.Player, slotPosition))
        {
            ClearSlot();
            this.button.SetActive(false);
        }
    }

    public void Update()
    {
        if (!GameManager.Instance.CanUnset(playerSide, slotPosition))
        {
            canHover = false;
        }
        else
        {
            canHover = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!canHover || button == null) return;
        
        PlayCardData cardData = GameManager.Instance.GetCardOnSlot(playerSide, slotPosition);
        
        if (cardData != null)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!canHover || button == null) return;
        button.SetActive(false);
    }
}
