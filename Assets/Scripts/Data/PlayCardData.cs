using System;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.BasicCards;
using ScriptableObjects.Modifier;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PlayCardData : IComparable<PlayCardData>
    {
        [SerializeField]
        private BasicCard card;
        
        [SerializeField]
        private Modifier modifier;
        
        public Modifier Modifier { get => modifier; set => modifier = value; }
        
        public BasicCard Card { get => card; set => card = value; }
        
        public PlayCardData(BasicCard card, Modifier modifier)
        {
            this.card = card;
            this.modifier = modifier;
        }
        
        public string GetCardName()
        {
            return modifier == null ? card.GetCardName() : modifier.ModName + " " + card.GetCardName();
        }
        
        public string GetCardDescription()
        {
            //Add Mods
            return modifier == null ? card.GetCardName() : modifier.modDescription + "\n" + card.GetCardDescription();
        }

        public CardType GetCardType()
        {
            return card.GetCardType();
        }
        
        public void PlayCard(GameSlot ownSlot, GameSlot enemySlot)
        {
            //Add Mods
            card.Play(ownSlot, enemySlot);
        }
        
        public int CompareTo(PlayCardData otherCardData)
        {
            //lt 0 current shit is smaller
            //0 current shit is same
            //gt 0 current shit is bigger

            switch (otherCardData.card.cardType)
            {
                case CardType.Interfere:
                    
                    if (this.card.cardType != CardType.Interfere)
                    {
                        return 1;
                    }
                    InterfereCard currentCard = (InterfereCard)this.card;
                    InterfereCard otherCard = (InterfereCard)otherCardData.card;
                    
                    return currentCard.priority.CompareTo(otherCard.priority);
                case CardType.Defense:
                    if (this.card.cardType == CardType.Interfere)
                    {
                        return -1;
                    }
                    if (this.card.cardType == CardType.Attack)
                    {
                        return 1;
                    }

                    return 0;
                case CardType.Attack:
                    if (this.card.cardType == CardType.Interfere)
                    {
                        return -1;
                    }
                    if (this.card.cardType == CardType.Defense)
                    {
                        return -1;
                    }
                    return 0;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}