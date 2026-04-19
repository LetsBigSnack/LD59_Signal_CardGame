using Data;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class UISlotManager : MonoBehaviour
{
    public static UISlotManager Instance;

    [SerializeField]
    private UISlot[] playerSlots;
    [SerializeField]
    private UISlot[] enemySlots;
    [SerializeField]
    private GameManager gameManager;

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
        gameManager.OnCardRemoved += HandleCardRemoved;
        gameManager.OnCardAdded += HandleCardAdded;
    }

    private void OnDisable()
    {
        gameManager.OnCardRemoved -= HandleCardRemoved;
        gameManager.OnCardAdded -= HandleCardAdded;
    }

    private void HandleCardAdded(GameSlot gameSlot)
    {
        GetSlot(gameSlot).SetCard(gameSlot.PlayCardData);
    }
    private void HandleCardRemoved(GameSlot gameSlot)
    {
        GetSlot(gameSlot).ClearSlot();
    }

    private UISlot GetSlot(GameSlot gameSlot)
    {
        return gameSlot.PlayerSide == PlayerSide.Player ? playerSlots[(int)gameSlot.SlotPosition] : enemySlots[(int)gameSlot.SlotPosition];
    }
}
