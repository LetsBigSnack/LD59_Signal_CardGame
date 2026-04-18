using UnityEngine;

namespace Data
{
    public class GameSlot
    {
        [SerializeField] private PlayCardData _playCardData;
        [SerializeField] private SlotPosition _slotPosition;
        [SerializeField] private PlayerSide _playerSide;
    }
}