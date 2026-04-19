using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using ScriptableObjects.Deck;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;




namespace Data
{
    
    [Serializable]
    public class PlayerCards
    {
        [SerializeField]
        private DeckList deckList;
        
        [SerializeField] 
        private List<PlayCardData> deck = new List<PlayCardData>();
        
        [SerializeField] 
        private List<PlayCardData> hand = new List<PlayCardData>();
        
        [SerializeField] 
        private List<PlayCardData> set = new List<PlayCardData>();
        
        [SerializeField] 
        private List<PlayCardData> graveyard = new List<PlayCardData>();

        private PlayCardData cardDrawn;

        public List<PlayCardData> Hand
        {
            get => hand;
            set => hand = value;
        }
        public DeckList GetDeckList
        {
            get => deckList;
        }
        public List<PlayCardData> GetGraveyard
        {
            get => graveyard;
        }

        public event Action<PlayCardData> OnDraw;

        
        public List<PlayCardData> Set
        {
            get => set;
            set => set = value;
        }
        
        public void InitializeDeck()
        {
            
            deckList = Object.Instantiate<DeckList>(deckList);
            deck = new List<PlayCardData>();
            hand = new List<PlayCardData>();
            set = new List<PlayCardData>();
            graveyard = new List<PlayCardData>();
            
            foreach (PlayCardData card in deckList.cards)
            {
                deck.Add(card);
            }
            
            deck.Shuffle();
            
        }
        
        
        public void Draw(int amount)
        {
            
            for (int i = 0; i < amount; i++)
            {
                if (deck.Count == 0)
                {
                    ShuffleGraveyardBack();
                }
                cardDrawn = deck.Pop();
                hand.Add(cardDrawn);
                OnDraw?.Invoke(cardDrawn);
            }
        }

        public bool SetCard(PlayCardData setCard)
        {
            if (hand.Find(card => card.Equals(setCard)) == null)
            {
                return false;
            }
            
            hand.Remove(setCard);
            set.Add(setCard);
            
            return true;
        }

        public bool UnsetCard(PlayCardData unsetCard)
        {
            if (set.Find(card => card.Equals(unsetCard)) == null)
            {
                return false;
            }
            
            set.Remove(unsetCard);
            hand.Add(unsetCard);
            
            return true;
        }
        
        public void PlayCard(PlayCardData playCard, GameSlot ownSlot, GameSlot enemySlot)
        {
            playCard.PlayCard(ownSlot, enemySlot);
        }

        public void ResolveCard(PlayCardData playCard)
        {
            graveyard.Add(playCard);
            set.Remove(playCard);
        }
        
        private void ShuffleGraveyardBack()
        {
            deck = deck.Concat(graveyard).ToList();
            graveyard = new List<PlayCardData>();
            deck.Shuffle();
        }

        public void DrawUntil(int untilAmount)
        {
            int cardDiff = untilAmount - hand.Count;
            
            if (cardDiff <= 0)
            {
                return;
            }
            
            Draw(cardDiff);
        }
    }
}