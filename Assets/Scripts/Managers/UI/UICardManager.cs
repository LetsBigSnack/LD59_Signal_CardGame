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
    private Transform handContainer;

    [SerializeField]
    private List<CardTypeSprite> cardTypeSprite;

    [SerializeField] 
    private UICardCollectionView collectionView;
    [SerializeField] 
    private GameObject window;

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
        player.PlayerCards.OnDraw += CreateCardUI;
    }

    private void OnDisable()
    {
        player.PlayerCards.OnDraw -= CreateCardUI;
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

    public void OpenDeck()
    {
        window.SetActive(true);
        collectionView.ShowCards(player.PlayerCards.GetDeckList.cards);
    }

    public void OpenGraveyard()
    {
        window.SetActive(true);
        collectionView.ShowCards(player.PlayerCards.GetGraveyard);
    }

    public void CloseWindow()
    {
        window.SetActive(false);
    }

    [Serializable]
    private class CardTypeSprite
    {
        public CardType cardType;
        public Sprite sprite;
    }
}
