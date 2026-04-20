using System.Collections.Generic;
using Data;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Managers
{
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
        
        [SerializeField] private List<Player> enemyList;
        [SerializeField] private Player currentEnemy;
        
        public Player GetCurrentEnemy()
        {
            return currentEnemy;
        }

        //TODO: dont create enemy instances -> fill current enemy script with new data (keep event actions alive)

        public void MakeMove()
        {
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position1);
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position2);
            GameManager.Instance.SetCardToSlot(currentEnemy.PlayerCards.Hand[0], PlayerSide.Enemy, SlotPosition.Position3);
        }
    }
}