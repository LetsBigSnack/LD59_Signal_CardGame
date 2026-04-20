using Data;
using ScriptableObjects.Deck;
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


    public PlayCardData GetCardData()
    {
        return cardData;
    }

    private void Awake()
    {
        originalScale = cardVisual.localScale;
        originalPosition = cardVisual.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
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

    public void SetSelected(bool value)
    {
        isSelected = value;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (isHovered || isSelected)
        {
            cardCanvas.overrideSorting = true;
            cardCanvas.sortingOrder = 100;

            cardVisual.localScale = originalScale * hoverScale;
            cardVisual.localPosition = originalPosition + new Vector3(0, hoverHeight, 0);
        }
        else
        {
            cardCanvas.overrideSorting = false;

            cardVisual.localScale = originalScale;
            cardVisual.localPosition = originalPosition;
        }
    }
}
