using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects.BasicCards;
using ScriptableObjects.Deck;
using ScriptableObjects.Modifier;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{

    [Serializable]
    public class ModifierCategory
    {
        [SerializeField]
        private CardType cardType;
        
        [SerializeField]
        private List<Modifier> modifiers;


        public CardType CardType
        {
            get => cardType;
            set => cardType = value;
        }

        public List<Modifier> Modifiers
        {
            get => modifiers;
            set => modifiers = value;
        }
        
    }
    
    
    public class CardManager : MonoBehaviour
    {
        public static CardManager Instance;
        
        [SerializeField] private List<ModifierCategory> modifierCategories;
        [SerializeField] private List<BasicCard> basicCards;

        [SerializeField] private float commonChance = 0.6f;
        [SerializeField] private float uncommonChance = 0.25f;
        [SerializeField] private float rareChance = 0.1f;
        [SerializeField] private float legendaryChance = 0.05f;
        [SerializeField] private float modifierChance = 0.5f;
            
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private CardRarity GetRandomCardRarity()
        {
            float roll = Random.value;

            if (roll < commonChance)
                return CardRarity.Common;

            if (roll < commonChance + uncommonChance)
                return CardRarity.Uncommon;
            
            if (roll < commonChance + uncommonChance + rareChance)
                return CardRarity.Rare;
            
            return CardRarity.Legendary;
        }

        private bool GetsModifier()
        {
            return Random.value < modifierChance;
        }
        
        public PlayCardData GetRandomPlayCard()
        {
            CardRarity cardRarity = GetRandomCardRarity();
            
            BasicCard baseCard = basicCards.Where(card => card.CardRarity ==  cardRarity)
                .OrderBy(card => Guid.NewGuid())
                .First();

            Modifier baseMod = null;
            
            if (GetsModifier())
            {
                ModifierCategory modCat = modifierCategories.First(cat => cat.CardType == baseCard.cardType);
                baseMod = modCat.Modifiers.OrderBy(card => Guid.NewGuid()).FirstOrDefault();
            }
            
            PlayCardData cardPlay = new PlayCardData(baseCard, baseMod);
            
            return cardPlay;
            
        }

        public DeckList CreateDeck(int size)
        {
            List<PlayCardData> deck = new List<PlayCardData>();
            DeckList deckList = ScriptableObject.CreateInstance<DeckList>();
            for (int i = 0; i < size; i++)
            {
                deck.Add(GetRandomPlayCard());
            }
            
            deckList.cards = deck;
            
            return deckList;
        }
        
        
    }
}