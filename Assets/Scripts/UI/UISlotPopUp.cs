using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlotPopUp : MonoBehaviour
{
    public static UISlotPopUp Instance;

    [SerializeField] 
    private GameObject panel;
    [SerializeField]
    private UIClickBlocker clickBlocker;

    private PlayCardData currentCard;
    private UICard currentUICard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenSlotPopup(UICard uiCard, PointerEventData eventData)
    {
        currentCard = uiCard.GetCardData();
        currentUICard = uiCard;
        currentUICard.SetSelected(true);

        panel.SetActive(true);
        clickBlocker.gameObject.SetActive(true);

        clickBlocker.OnClicked = CloseSlotPopUp;

        panel.transform.position = eventData.position;
    }

    public void CloseSlotPopUp()
    {
        if (currentUICard != null)
        {
            currentUICard.SetSelected(false);
            currentUICard = null;
        }

        clickBlocker.OnClicked = null;
        clickBlocker.gameObject.SetActive(false);

        panel.SetActive(false);
    }

    public void AddToSlot(int slotIndex)
    {
        if (currentCard == null) return;

        UISlotManager.Instance.PlaceCard(slotIndex, currentCard);

        CloseSlotPopUp();
    }
}
