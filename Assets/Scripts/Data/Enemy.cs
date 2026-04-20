using Managers;
using ScriptableObjects.Deck;
using UnityEngine;

namespace Data
{
    public class Enemy : Player
    {
        [SerializeField] private EnemyPlayStyle _enemyPlayStyle;

        public void SetSettings(EnemyPlayStyle newStyle, DeckList deckList)
        {
            base.PlayerCards.GetDeckList = deckList;
            _enemyPlayStyle =  newStyle;
            PlayerCards.CloneDeckList();
        }
        
        public void MakeMove(GameSlot own, GameSlot opponent, bool isSetter)
        {
            
            
            
        }
        
    }
}