using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResolvePopupController : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image modifierImage;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private GameObject resolveParent;

    [SerializeField]
    private Animator _anim;

    public Animator Setup(Sprite iconSprite, Sprite modifierSprite, int amount, string title)
    {
        resolveParent.SetActive(true);
        if(modifierSprite == null)
        {
            modifierImage.color = new Color(0,0,0,0);
        } 
        else
        {
            modifierImage.color = new Color(1,1,1,1);
        }
        iconImage.sprite = iconSprite;
        modifierImage.sprite = modifierSprite;
        amountText.text = amount.ToString();
        titleText.text = title;
        _anim.Play("ResolvePopupOpen");
        SoundManager.Instance.PlaySound("resolve", true);

        return _anim;
    }

    public void disableObject()
    {
        resolveParent.SetActive(false);
    }
}
