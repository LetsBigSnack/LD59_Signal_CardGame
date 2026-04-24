using Data;
using ScriptableObjects.Deck;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI cardNameTxt;

    [SerializeField]
    private TextMeshProUGUI cardDiscTxt;

    [SerializeField]
    private TextMeshProUGUI cardTypeTxt;
    [SerializeField]
    private CardType cardType;

    [SerializeField]
    private Image cardImg;

    [SerializeField] 
    private RectTransform cardVisual;

    [SerializeField]
    private RectTransform cardUIVisual;

    [SerializeField]
    private float time = 0f;

    [SerializeField]
    private int lerpSpeed = 1;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    [SerializeField] 
    private float hoverScale = 1.8f;
    [SerializeField] 
    private float hoverHeight = 148f;

    [SerializeField]
    private Canvas cardCanvas;

    private PlayCardData cardData;
    private bool isHovered;
    private bool isSelected;

    [SerializeField]
    private bool canHover = true;


    public PlayCardData GetCardData()
    {
        return cardData;
    }

    private void Awake()
    {
        originalScale = cardVisual.localScale;
        originalPosition = cardVisual.localPosition;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            UISlotPopUp.Instance.OpenSlotPopup(this, gameObject.transform.position);
        }
    }

    public void Setup(PlayCardData playCardData, Sprite sprite)
    {
        cardData = playCardData;

        cardNameTxt.text = playCardData.GetCardName();
        cardDiscTxt.text = playCardData.GetCardDescription();
        cardTypeTxt.text = playCardData.GetCardType().ToString();

        cardType = playCardData.GetCardType();

        cardImg.sprite = sprite;
    }

    public void Update()
    {
        UpdateVisual();
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (!canHover) return;

        time += Time.deltaTime * lerpSpeed;

        if (isHovered || isSelected)
        {
            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = true;
                cardCanvas.sortingOrder = 100;
            }

            cardVisual.localScale = Vector3.Lerp(originalScale, originalScale * hoverScale, time);
            cardVisual.localPosition = Vector3.Lerp(originalPosition , originalPosition + new Vector3(0, hoverHeight, 0), time);
            cardUIVisual.localScale = Vector3.Lerp(originalScale, originalScale * hoverScale, time);
            cardUIVisual.localPosition = Vector3.Lerp(originalPosition, originalPosition + new Vector3(0, hoverHeight, 0), time);
        }
        else
        {
            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = false;
            }
            time = 0;
            cardVisual.localScale = originalScale;
            cardVisual.localPosition = originalPosition;
            cardUIVisual.localScale = originalScale;
            cardUIVisual.localPosition = originalPosition;
        }
    }
}
