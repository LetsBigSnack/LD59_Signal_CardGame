using System;
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
        
        private bool _isDead;
        
        
        
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