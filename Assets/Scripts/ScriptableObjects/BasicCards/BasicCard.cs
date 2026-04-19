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

        public virtual void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            //TOOD: add effect
        }
    
    }
}
