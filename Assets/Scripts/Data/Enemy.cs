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
            
            return RespondCards(gameSlots);

        }

        public List<PlayCardData> SetCards(List<GameSlot> gameSlots)
        {
            List<PlayCardData> handCards = PlayerCards.Hand.OrderBy(c=> Guid.NewGuid()).ToList();
            List<PlayCardData> cards = new List<PlayCardData>();
            List<CardChance> chances = _enemyPlayStyle.CardChances.ToList();
            chances = chances.OrderBy(c=> Guid.NewGuid()).ToList();
            
            
            while (cards.Count < 3)
            {
                GetRandomCard(chances, handCards, cards);
            }
            
            return cards;
        }

        public void GetRandomCard(List<CardChance> chances, List<PlayCardData> handCards, List<PlayCardData> cards)
        {
            while (true)
            {
                foreach (CardChance chance in chances)
                {
                    float rng = Random.Range(0f, 1f);

                    if (rng < chance.Chance)
                    {
                        PlayCardData cardData = handCards.FirstOrDefault(c => c.Card.cardType == chance.CardType && !cards.Contains(c));
                        if (cardData != null)
                        {
                            cards.Add(cardData);
                            return;
                        }
                    }
                }
            }
            
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


            //Look at the Slots and if attack choose Defend or Interfere
            
            List<PlayCardData> oppCards = gameSlots.Where(c => c.PlayerSide == PlayerSide.Player).OrderBy(ob => ob.SlotPosition).Select(a => a.PlayCardData).ToList();

            foreach (PlayCardData oppCard in oppCards)
            {
                if (oppCard == null || oppCard.Card == null)
                {
                    GetRandomCard(chances, handCards, cards);
                    continue;
                }
                
                if (oppCard.GetCardType() == CardType.Attack)
                {

                    List<PlayCardData> possibleCards = handCards
                        .Where(c => (c.Card.cardType == CardType.Defense || c.Card.cardType == CardType.Interfere) &&
                                    !cards.Contains(c)).ToList();
                    
                    possibleCards.OrderBy(c=> Guid.NewGuid()).ToList();
                    
                    PlayCardData chosenCard = possibleCards.FirstOrDefault();
                    
                    if(chosenCard != null)
                    {
                        cards.Add(chosenCard);
                    }
                    else
                    {
                        GetRandomCard(chances, handCards, cards);
                    }
                }
                else
                {
                    GetRandomCard(chances, handCards, cards);
                }
            }
            
            return cards;
        }
        
    }
}