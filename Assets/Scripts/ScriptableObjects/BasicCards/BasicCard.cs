using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
    
    public class BasicCard : ScriptableObject
    {
        [Header("Card Info")]
        public string cardName;
        public string cardDescription;
        [SerializeField]
        public CardType cardType;
        
        [SerializeField]
        private Sprite cardSprite;
        
        [SerializeField]
        private CardRarity rarity;

        public CardRarity CardRarity
        {
            get => rarity;
            set => rarity = value;
        }
        
        
        public string GetCardName()
        {
            return cardName;
        }
    
        public virtual string GetCardDescription()
        {
            return cardDescription;
        }

        public CardType GetCardType()
        {
            return cardType;
        }

        public Sprite GetCardSprite()
        {
            return cardSprite;
        }

        public Sprite SetCardSprite(Sprite sprite)
        {
            return cardSprite = sprite;
        }

        public virtual void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            //TOOD: add effect
        }
    
    }
}
