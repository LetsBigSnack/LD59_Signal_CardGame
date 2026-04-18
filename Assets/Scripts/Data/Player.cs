using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private string playerName;
        
        [FormerlySerializedAs("deck")] [SerializeField]
        private PlayerCards playerCards;

        [SerializeField] 
        private int maxHealth;
        
        [SerializeField] 
        private int currentHealth;
        
        
        public PlayerCards PlayerCards { 
            set => playerCards = value;
            get => playerCards;
        }
        
        public void ResetPlayer()
        {
            currentHealth = maxHealth;
            playerCards.InitializeDeck();
        }
        
        
        
    }
}