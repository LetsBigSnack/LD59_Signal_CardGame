using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardRemove : UICard
{
    [SerializeField]
    public GameObject button;

    private bool hasBeenClicked = false;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (hasBeenClicked) return;
        button.SetActive(true);
    }

    public void Update()
    {
        if (hasBeenClicked)
        {
            button.SetActive(false);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        button.SetActive(false);
    }

    public void RemoveCardFromDeck()
    {
        PlayerManager.Instance.Player.PlayerCards.RemoveFromDeck(GetCardData());
        RewardManager.Instance.ShowRemoveCard();
        hasBeenClicked = true;
    }
}
