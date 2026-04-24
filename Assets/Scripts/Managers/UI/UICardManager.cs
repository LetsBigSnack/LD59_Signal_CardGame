using Data;
using ScriptableObjects.Deck;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
    private GameObject cardDeckRemoveablePrefab;

    [SerializeField]
    private Transform handContainer;

    [SerializeField]
    private float drawSpeed = 0.1f;

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

    public void CreateCardUI(PlayCardData playCardData, bool playAnim)
    {
        GameObject card = Instantiate(cardHandPrefab, handContainer);
        if (playAnim)
        {
            SoundManager.Instance.PlaySound("cardDraw");
            card.GetComponentInChildren<Animator>().Play("HandFlip");
        }

        UICard uiCard = card.GetComponent<UICard>();
        try {
            Sprite sprite = playCardData.Card.GetCardSprite();
            uiCard.Setup(playCardData, sprite);
        }
        catch(Exception e)
        {
            Debug.Log(playCardData.Card.name);
        }
        

   
    }

    public GameObject CreateCardUI(PlayCardData playCardData, Transform parent, bool isRemovable = false)
    {
        GameObject card = Instantiate(isRemovable? cardDeckRemoveablePrefab : cardDeckPrefab, parent);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = playCardData.Card.GetCardSprite(); ;

        uiCard.Setup(playCardData, sprite);

        return card;
    }

    public GameObject CreateSlotCardUI(PlayCardData playCardData, Transform parent)
    {
        GameObject card = Instantiate(cardSlotPrefab, parent);

        UICard uiCard = card.GetComponent<UICard>();
        Sprite sprite = playCardData.Card.GetCardSprite();

        uiCard.Setup(playCardData, sprite);

        return card;
    }


    public void RefreshHandUI(bool isNewHand)
    {
            StartCoroutine(DrawCards(isNewHand));
    }

    [Serializable]
    private class CardTypeSprite
    {
        public CardType cardType;
        public Sprite sprite;
    }

    public IEnumerator DrawCards(bool playAnim)
    {
        Debug.Log(DateTime.Now);
        foreach (Transform child in handContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (PlayCardData card in player.PlayerCards.Hand.ToList())
        {
            CreateCardUI(card, playAnim);
            yield return new WaitForSeconds(playAnim? drawSpeed : 0);
        }
    }
}
