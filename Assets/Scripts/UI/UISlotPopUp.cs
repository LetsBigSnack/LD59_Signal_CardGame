using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UISlotPopUp : MonoBehaviour
{
    public static UISlotPopUp Instance;

    [SerializeField] 
    private GameObject panel;
    [SerializeField]
    private UIClickBlocker clickBlocker;
    [SerializeField]
    public Vector3 offset;

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

    public void OpenSlotPopup(UICard uiCard, Vector3 pos)
    {
        currentCard = uiCard.GetCardData();
        currentUICard = uiCard;
        currentUICard.SetSelected(true);

        panel.SetActive(true);
        clickBlocker.gameObject.SetActive(true);

        clickBlocker.OnClicked = CloseSlotPopUp;

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.position = pos + offset;
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

        GameManager.Instance.SetCardToSlot(currentCard, PlayerSide.Player, (SlotPosition)slotIndex);

        CloseSlotPopUp();
    }


}
