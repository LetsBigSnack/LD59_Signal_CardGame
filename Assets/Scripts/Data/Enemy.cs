using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using ScriptableObjects.Deck;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    public class Enemy : Player
    {
        [SerializeField] private EnemyPlayStyleSettings _enemyPlayStyle;
        [SerializeField] private int dangerHealth = 4;
        
        public void SetSettings(EnemyPlayStyleSettings newStyle, DeckList deckList)
        {
            base.PlayerCards.GetDeckList = deckList;
            _enemyPlayStyle =  newStyle;
            PlayerCards.CloneDeckList();
        }
        
        public List<PlayCardData> MakeMove(List<GameSlot> gameSlots, bool isSetter)
        {

            if (isSetter)
            {
                return SetCards(gameSlots);
            }
            
            
            
            
            return null;

        }

        public List<PlayCardData> SetCards(List<GameSlot> gameSlots)
        {
            List<PlayCardData> handCards = PlayerCards.Hand.OrderBy(c=> Guid.NewGuid()).ToList();
            List<PlayCardData> cards = new List<PlayCardData>();
            List<CardChance> chances = _enemyPlayStyle.CardChances.ToList();
            chances = chances.OrderBy(c=> Guid.NewGuid()).ToList();
            
            int tries = 0;
            
            while (cards.Count < 3 || tries < 100)
            {
                foreach (CardChance chance in chances)
                {
                    tries++;
                    float rng = Random.Range(0f, 1f);

                    if (rng < chance.Chance)
                    {
                        PlayCardData cardData = handCards.FirstOrDefault(c => c.Card.cardType == chance.CardType && !cards.Contains(c));
                        if (cardData != null)
                        {
                            cards.Add(cardData);
                        }
                    }
                    
                }
            }
            
            return cards;
        }
        
        public List<PlayCardData> RespondCards(List<GameSlot> gameSlots)
        {
            if (CurrentHealth > dangerHealth)
            {
                return  SetCards(gameSlots);
            }
            
            
            List<PlayCardData> handCards = PlayerCards.Hand.OrderBy(c=> Guid.NewGuid()).ToList();
            List<PlayCardData> cards = new List<PlayCardData>();
            List<CardChance> chances = _enemyPlayStyle.CardChances.ToList();
            chances = chances.OrderBy(c=> Guid.NewGuid()).ToList();
            
            
            
            return cards;
        }
        
    }
}