using Data;
using ScriptableObjects.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


public class UICardManager : MonoBehaviour
{
    public static UICardManager Instance;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject cardHandPrefab;
    [SerializeField]
    private GameObject cardDeckPrefab;
    [SerializeField]
    private GameObject cardSlotPrefab;

    [SerializeField]
    private Transform handContainer;

    [SerializeField]
    private List<CardTypeSprite> cardTypeSprite;

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

    private void OnEnable()
    {
        player.PlayerCards.OnHandChange += RefreshHandUI;
    }

    private void OnDisable()
    {
        player.PlayerCards.OnHandChange -= RefreshHandUI;
    }

    public Sprite GetSprite(CardType cardType)
    {
        CardTypeSprite cardTypeSpriteHelper = cardTypeSprite.Where(c => c.cardType == cardType).FirstOrDefault();

        if(cardTypeSpriteHelper == null)
            return null;

        return cardTypeSpriteHelper.sprite;
    }

    public void CreateCardUI(PlayCardData playCardData)
    {
        GameObject card = Instantiate(cardHandPrefab, handContainer);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = GetSprite(playCardData.GetCardType());

        uiCard.Setup(playCardData, sprite);
    }

    public GameObject CreateCardUI(PlayCardData playCardData, Transform parent)
    {
        GameObject card = Instantiate(cardDeckPrefab, parent);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = GetSprite(playCardData.GetCardType());

        uiCard.Setup(playCardData, sprite);

        return card;
    }

    public GameObject CreateSlotCardUI(PlayCardData playCardData, Transform parent)
    {
        GameObject card = Instantiate(cardSlotPrefab, parent);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = GetSprite(playCardData.GetCardType());

        uiCard.Setup(playCardData, sprite);

        return card;
    }


    public void RefreshHandUI()
    {
        foreach (Transform child in handContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (PlayCardData card in player.PlayerCards.Hand)
        {
            CreateCardUI(card);
        }
    }

    [Serializable]
    private class CardTypeSprite
    {
        public CardType cardType;
        public Sprite sprite;
    }
}
