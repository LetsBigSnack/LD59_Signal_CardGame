using Managers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardAdd : UICard
{
    [SerializeField]
    public GameObject button;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        button.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        button.SetActive(false);
    }

    public void SelectCardToAddToDeck()
    {
        RewardManager.Instance.SelectReward(GetCardData());
    }
}
