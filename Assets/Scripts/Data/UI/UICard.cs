using Data;
using ScriptableObjects.Deck;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    private void Awake()
    {
        originalScale = cardVisual.localScale;
        originalPosition = cardVisual.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        cardCanvas.overrideSorting = true;
        cardCanvas.sortingOrder = 100;

        cardVisual.localScale = originalScale * hoverScale;
        cardVisual.localPosition = originalPosition + new Vector3(0, hoverHeight, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardVisual.localScale = originalScale;
        cardVisual.localPosition = originalPosition;

        cardCanvas.overrideSorting = false;
    }
    public void Setup(PlayCardData playCardData, Sprite sprite)
    {
        cardNameTxt.text = playCardData.GetCardName();
        cardDiscTxt.text = playCardData.GetCardDescription();
        cardTypeTxt.text = playCardData.GetCardType().ToString();

        cardType = playCardData.GetCardType();

        cardImg.sprite = sprite;
    }
}
