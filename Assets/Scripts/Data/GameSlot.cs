using System;
using Managers;
using ScriptableObjects.Modifier;
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
        [SerializeField] private int heal;
        [SerializeField] private int prioTurns;
        [SerializeField] private int cardDraw;
        [SerializeField] private ModifierType  ownModifierType = ModifierType.None;
        [SerializeField] private ModifierType  oppositeModifierType = ModifierType.None;
        [SerializeField] private GameSlot oppositeGameSlot;
        
        private bool _isBlocked;
        
        public PlayCardData PlayCardData { get => playCardData; set => playCardData = value; }
        public SlotPosition SlotPosition { get => slotPosition; set => slotPosition = value; }
        public PlayerSide PlayerSide { get => playerSide; set => playerSide = value; }
        public ModifierType OwnModifierType { get => ownModifierType; set => ownModifierType = value; }
        public ModifierType OppositeModifierType { get => oppositeModifierType; set => oppositeModifierType = value; }
        public GameSlot OppositeGameSlot { get => oppositeGameSlot; set => oppositeGameSlot = value; }
        
        public int PrioTurns { get => prioTurns; set => prioTurns = value; }
        public int CardDraw { get => cardDraw; set => cardDraw = value; }

        public int Heal
        {
            get => heal;
            set => heal = value;
        }

        public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }
        public GameSlot(SlotPosition slotPosition, PlayerSide playerSide, PlayCardData playCardData = null)
        {
            this.slotPosition = slotPosition;
            this.playerSide = playerSide;
            this.playCardData = playCardData;
            this._isBlocked = false;
            this.defense = 0;
            this.attack = 0;
            this.heal = 0;
            this.prioTurns = 0;
            this.cardDraw = 0;
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
            
            if (ownModifierType == ModifierType.Firewall && attack > 0)
            {
                int blockedDamage = defense - Mathf.Max(defense - attack, 0);
                GameManager.Instance.AddToResolveQueue(new ResolveObject(ResolveType.Damage, blockedDamage, oppositeGameSlot, this));
                //GameManager.Instance.GetOppositePlayer(playerSide).TakeDamage(blockedDamage);
            }
            
            int damage = oppositeModifierType == ModifierType.Backdoor? attack : attack - defense;
            
            if (damage == 0)
            {
                return;
            }
            
            if (damage > 0)
            {
                // own (opposite) opposite ich bin 
                
                //GameManager.Instance.Players[playerSide].TakeDamage(damage);
                GameManager.Instance.AddToResolveQueue(new ResolveObject(ResolveType.Damage, damage, this, oppositeGameSlot));
                
                if (oppositeModifierType == ModifierType.Phising)
                {
                    oppositeGameSlot.AddHeal(damage);
                }
            }
            else
            {
                if (ownModifierType == ModifierType.BackUp)
                {
                    AddHeal(damage);
                }
            }
        }

        public void AddHeal(int heal)
        {
            this.heal += heal;
        }

        public void AddPrio(int turns)
        {
            prioTurns +=  turns;
        }

        public void AddCardDraw(int draw)
        {
            cardDraw += draw;
        }
    }
}