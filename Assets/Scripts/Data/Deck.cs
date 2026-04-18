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
    public class Deck
    {
        [SerializeField]
        private DeckList deckList;
        
        [SerializeField] 
        private List<PlayCardData> deck = new List<PlayCardData>();
        
        [SerializeField] 
        private List<PlayCardData> hand = new List<PlayCardData>();
        
        [SerializeField] 
        private List<PlayCardData> graveyard = new List<PlayCardData>();

        private PlayCardData cardDrawn;

        public List<PlayCardData> Hand
        {
            get => hand;
            set => hand = value;
        }

        public event Action<PlayCardData> OnDraw;

        public void InitializeDeck()
        {
            
            deckList = Object.Instantiate<DeckList>(deckList);
            deck = new List<PlayCardData>();
            hand = new List<PlayCardData>();
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

        public void PlayCard(PlayCardData playCard)
        {
            playCard.PlayCard();
            graveyard.Add(playCard);
            hand.Remove(playCard);
        }

        private void ShuffleGraveyardBack()
        {
            deck = deck.Concat(graveyard).ToList();
            graveyard = new List<PlayCardData>();
            deck.Shuffle();
        }
    }
}