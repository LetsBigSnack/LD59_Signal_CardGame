using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        [SerializeField]
        private string playerName;
        
        [FormerlySerializedAs("deck")] [SerializeField]
        private PlayerCards playerCards;

        [SerializeField] 
        private int maxHealth;
        
        [SerializeField] 
        private int currentHealth;
        
        private bool _isDead;
        
        
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
        
        public PlayerCards PlayerCards { 
            set => playerCards = value;
            get => playerCards;
        }
        
        public void ResetPlayer()
        {
            currentHealth = maxHealth;
            playerCards.InitializeDeck();
        }


        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                _isDead = true;
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}