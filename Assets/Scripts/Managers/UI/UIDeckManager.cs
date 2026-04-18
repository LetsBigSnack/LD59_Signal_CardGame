using Data;
using ScriptableObjects.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIDeckManager : MonoBehaviour
{
    public static UIDeckManager Instance;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject cardPrefab;

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
        player.Deck.OnDraw += CreateCardUI;
    }

    private void OnDisable()
    {
        player.Deck.OnDraw -= CreateCardUI;
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
        GameObject card = Instantiate(cardPrefab, handContainer);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = GetSprite(playCardData.GetCardType());

        uiCard.Setup(playCardData, sprite);
    }

    [Serializable]
    private class CardTypeSprite
    {
        public CardType cardType;
        public Sprite sprite;
    }
}
