using System;
using System.Collections.Generic;
using Data;
using ScriptableObjects.Deck;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Managers
{

    public enum EnemyPlayStyle
    {
        Balanced,
        Aggressive,
        Defensive,
        Interfering
    }
    
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;
        
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
        
       [SerializeField] private Enemy currentEnemy;

        public void CreateNewEnemy()
        {
            DeckList newDeckList =
                CardManager.Instance.CreateDeck(PlayerManager.Instance.Player.PlayerCards.GetDeckList.cards.Count);
            EnemyPlayStyle newStyle = GetRandomPlaystyle();
            currentEnemy.SetSettings(newStyle, newDeckList);
            
        }

        private EnemyPlayStyle GetRandomPlaystyle()
        {
            EnemyPlayStyle[] styles = (EnemyPlayStyle[])System.Enum.GetValues(typeof(EnemyPlayStyle));
            return styles[UnityEngine.Random.Range(0, styles.Length)];
        }


        public Player GetCurrentEnemy()
        {
            return currentEnemy;
        }

        //TODO: dont create enemy instances -> fill current enemy script with new data (keep event actions alive)

        public void MakeMove(List<GameSlot> gameSlots, bool isSetter)
        {

            List<PlayCardData> cardChoicePos1 = currentEnemy.MakeMove(gameSlots, isSetter);
            
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position1);
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position2);
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position3);
        }
    }
}