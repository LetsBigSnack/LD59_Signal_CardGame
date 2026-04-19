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
        private List<Modifier> modifiers;


        public string GetCardName()
        {
            //Add Mods
            return card.GetCardName();
        }
        
        public string GetCardDescription()
        {
            //Add Mods
            return card.GetCardDescription();
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