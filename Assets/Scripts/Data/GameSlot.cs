using System;
using UnityEngine;

namespace Data
{
    
    [Serializable]
    public class GameSlot
    {
        [SerializeField] private PlayCardData playCardData;
        [SerializeField] private SlotPosition slotPosition;
        [SerializeField] private PlayerSide playerSide;
        
        public PlayCardData PlayCardData { get => playCardData; set => playCardData = value; }
        public SlotPosition SlotPosition { get => slotPosition; set => slotPosition = value; }
        public PlayerSide PlayerSide { get => playerSide; set => playerSide = value; }
        
        public GameSlot(SlotPosition slotPosition, PlayerSide playerSide, PlayCardData playCardData = null)
        {
            this.slotPosition = slotPosition;
            this.playerSide = playerSide;
            this.playCardData = playCardData;
        }
        
    }
}