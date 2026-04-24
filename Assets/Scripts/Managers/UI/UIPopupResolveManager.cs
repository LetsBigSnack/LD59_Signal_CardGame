using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using System.Linq;
using ScriptableObjects.Modifier;
using UnityEngine;

[Serializable]
public class ResolveSlot
{
    public PlayerSide side;
    public SlotPosition position;
    public UIResolvePopupController resolvePopupController;
}

[Serializable]
public class PopupTypeIcon
{
    public ResolveType type;
    public Sprite icon;
}

[Serializable]
public class ModifierIcon
{
    public ModifierType type;
    public Sprite icon;
}

public class UIPopupResolveManager : MonoBehaviour
{
    [SerializeField]
    private List<ResolveObject> queueElements = new List<ResolveObject>();

    [SerializeField] private float timeToResolveCard = 0.4f;
    [SerializeField] private float timeAfterResolve = 0.5f;

    [SerializeField]
    private List<PopupTypeIcon> popupTypeIcons = new List<PopupTypeIcon>();

    [SerializeField]
    private List<ModifierIcon> modifierIcons = new List<ModifierIcon>();

    [SerializeField]
    private List<ResolveSlot> slots = new List<ResolveSlot>();

    private Dictionary<ResolveType, string> resolveNames = new Dictionary<ResolveType, string>();

    public void Awake()
    {
        GameManager.Instance.OnResolve += AddToResolveQueue;
        GameManager.Instance.OnResolveFinished += ResolveFinished;

        resolveNames.Add(ResolveType.Interfere, "Interfere");
        resolveNames.Add(ResolveType.Defend, "Hide");
        resolveNames.Add(ResolveType.Attack, "Attack");
        resolveNames.Add(ResolveType.Damage, "Damage");
        resolveNames.Add(ResolveType.Heal, "Heal");
        resolveNames.Add(ResolveType.CardDraw, "Draw");
    }
        
    private void AddToResolveQueue(ResolveObject resolve)
    {
        queueElements.Add(resolve);
    }

    private void ResolveFinished(bool finished)
    {
        if (finished)
        {
            StartCoroutine(StartCoolAnimation());
        }
        else
        {
            foreach(ResolveSlot slot in slots)
            {
                slot.resolvePopupController.disableObject();
            }
            queueElements.Clear();
        }
    }

    IEnumerator StartCoolAnimation()
    {
        Debug.Log("Starting cool animation");

        foreach (ResolveObject resolveObject in queueElements)
        {
            ResolveSlot slot = slots.FirstOrDefault(slot => slot.side == resolveObject.OwnSlot.PlayerSide && slot.position == resolveObject.OwnSlot.SlotPosition);
            ModifierIcon modifierIcon = modifierIcons.FirstOrDefault(icon => icon.type == resolveObject.OwnSlot.OwnModifierType);
            PopupTypeIcon popupTypeIcon = popupTypeIcons.FirstOrDefault(icon => icon.type == resolveObject.ResolveType);

            Animator anim = slot.resolvePopupController.Setup(popupTypeIcon.icon, modifierIcon?.icon, resolveObject.Number, resolveNames[resolveObject.ResolveType]);

            switch (resolveObject.ResolveType)
            {
                case ResolveType.Damage:
                    GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].TakeDamage(resolveObject.Number);
                    break;
                case ResolveType.Heal:
                    GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].HealLife(resolveObject.Number);
                    break;
            }
            //yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            yield return new WaitForSeconds(timeToResolveCard);
        }
        yield return new WaitForSeconds(timeAfterResolve);
        GameManager.Instance.ProceedToNextState();
        Debug.Log("Ending cool animation");
    }
        
}
