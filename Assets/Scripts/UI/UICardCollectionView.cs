using Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICardCollectionView : MonoBehaviour
{
    [SerializeField] 
    private Transform contentContainer;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Player enemy;
    [SerializeField]
    private GameObject uiContainerWindow;
    [SerializeField]
    private TextMeshProUGUI windowTitle;

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

    public void OpenDeck()
    {
        uiContainerWindow.SetActive(true);
        windowTitle.text = "Your Deck";
        ShowCards(player.PlayerCards.GetDeckList.cards);
    }

    public void OpenGraveyard()
    {
        uiContainerWindow.SetActive(true);
        windowTitle.text = "Your Graveyard";
        ShowCards(player.PlayerCards.GetGraveyard);
    }

    public void OpenEnemyDeck()
    {
        uiContainerWindow.SetActive(true);
        windowTitle.text = enemy.name + " Deck";
        ShowCards(enemy.PlayerCards.GetDeckList.cards);
    }

    public void OpenEnemyGraveyard()
    {
        uiContainerWindow.SetActive(true);
        windowTitle.text = enemy.name + " Graveyard";
        ShowCards(enemy.PlayerCards.GetGraveyard);
    }

    public void CloseWindow()
    {
        uiContainerWindow.SetActive(false);
    }
}
