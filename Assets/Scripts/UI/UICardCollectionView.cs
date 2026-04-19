using Data;
using System.Collections.Generic;
using UnityEngine;

public class UICardCollectionView : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;

    public void ShowCards(List<PlayCardData> cards)
    {
        Clear();

        foreach (PlayCardData card in cards)
        {
            UICardManager.Instance.CreateCardUI(card, contentContainer);
        }
    }

    private void Clear()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
