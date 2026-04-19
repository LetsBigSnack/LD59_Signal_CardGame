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
        [SerializeField] private int defense;
        [SerializeField] private int attack;
        
        private bool _isBlocked;
        
        public PlayCardData PlayCardData { get => playCardData; set => playCardData = value; }
        public SlotPosition SlotPosition { get => slotPosition; set => slotPosition = value; }
        public PlayerSide PlayerSide { get => playerSide; set => playerSide = value; }
        
        public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }
        public GameSlot(SlotPosition slotPosition, PlayerSide playerSide, PlayCardData playCardData = null)
        {
            this.slotPosition = slotPosition;
            this.playerSide = playerSide;
            this.playCardData = playCardData;
            this._isBlocked = false;
            this.defense = 0;
            this.attack = 0;
        }

        public void AddDefense(int amount)
        {
            this.defense += amount;
        }

        public void AddAttack(int amount)
        {
            this.attack += amount;
        }

        public void BlockSlot()
        {
            _isBlocked = true;
        }

        public void ResolveTurn()
        {
            int damage = attack - defense;

            if (damage > 0)
            {
                GameManager.Instance.Players[playerSide].TakeDamage(damage);
            }
            
        }
    }
}