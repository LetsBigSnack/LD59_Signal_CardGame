using UnityEngine;

namespace Data
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private string playerName;
        
        [SerializeField]
        private Deck deck;

        [SerializeField] 
        private int maxHealth;
        
        [SerializeField] 
        private int currentHealth;
        
        
        public Deck Deck { 
            set => deck = value;
            get => deck;
        }
        
        
    }
}