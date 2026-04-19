using Data;
using UnityEngine;

public class UISlotManager : MonoBehaviour
{
    public static UISlotManager Instance;

    [SerializeField]
    private UISlot[] playerSlots;
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
    }

    private void OnDisable()
    {
        gameManager.OnCardRemoved -= HandleCardRemoved;
    }

    public void PlaceCard(int slotIndex, PlayCardData card)
    {
        if (slotIndex < 0 || slotIndex >= playerSlots.Length)
            return;

        UISlot uiSlot = playerSlots[slotIndex];

        if (uiSlot.IsOccupied)
            return;

        GameManager.Instance.SetCardToSlot(card, PlayerSide.Player, (SlotPosition)(slotIndex));

        uiSlot.SetCard(card);
    }

    private void HandleCardRemoved(GameSlot gameSlot)
    {
        if (gameSlot.PlayerSide != PlayerSide.Player)
            return;

        int index = (int)gameSlot.SlotPosition;

        if (index < 0 || index >= playerSlots.Length)
            return;

        playerSlots[index].ClearSlot();
    }
}
