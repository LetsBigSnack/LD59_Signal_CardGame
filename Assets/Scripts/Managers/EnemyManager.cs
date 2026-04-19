using System.Collections.Generic;
using Data;
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
        
    }
}