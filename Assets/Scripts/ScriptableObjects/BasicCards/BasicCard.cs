using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    public class BasicCard : ScriptableObject
    {
        [Header("Card Info")]
        public string cardName;
        public string cardDescription;
        [SerializeField]
        public CardType cardType;
        
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

        public virtual void Play()
        {
            //TOOD: add effect
        }
    
    }
}
