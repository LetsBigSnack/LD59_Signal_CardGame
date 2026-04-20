using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class Player : MonoBehaviour
    {
        
        [SerializeField]
        private string playerName;

        [SerializeField]
        private PlayerSide playerSide;

        [FormerlySerializedAs("deck")] [SerializeField]
        private PlayerCards playerCards;

        [SerializeField] 
        private int maxHealth;
        
        [SerializeField] 
        private int currentHealth;
        
        private bool _isDead;

        public event Action<PlayerSide, int> OnLifeChanged;

        public int CurrentHealth
        {
            get => currentHealth;
        }
        public PlayerCards PlayerCards { 
            set => playerCards = value;
            get => playerCards;
        }

        private void Awake()
        {
            playerCards.CloneDeckList();
        }

        public void ResetPlayer()
        {
            currentHealth = maxHealth;
            _isDead = false;
            OnLifeChanged?.Invoke(playerSide, currentHealth);
            playerCards.InitializeDeck();
        }


        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            OnLifeChanged?.Invoke(playerSide, currentHealth < 0 ? 0 : currentHealth);
            
            if (currentHealth <= 0)
            {
                _isDead = true;
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void HealLife(int life)
        {
            currentHealth += life;
            
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            if (currentHealth > 0)
            {
                _isDead = false;
            }
            
            if (currentHealth <= 0)
            {
                _isDead = true;
            }
            
        }
    }
}