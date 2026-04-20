using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects.Deck;
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
    
    [Serializable]
    public class CardChance
    {
        [SerializeField] private CardType cardType;
        [SerializeField] private float chance;
        
        public CardType CardType => cardType;
        public float Chance => chance;
    }
    
    [Serializable]
    public class EnemyPlayStyleSettings
    {
        [SerializeField] private EnemyPlayStyle playStyle;
        [SerializeField] private List<CardChance> cardChances;
        public EnemyPlayStyle  PlayStyle => playStyle;
        public List<CardChance> CardChances => cardChances;
    }
    
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;
        
        [SerializeField]
        public List<EnemyPlayStyleSettings> SettingsList;
        
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
            EnemyPlayStyleSettings newStyle = GetRandomPlaystyle();
            currentEnemy.SetSettings(newStyle, newDeckList);
            
        }

        private EnemyPlayStyleSettings GetRandomPlaystyle()
        {
            EnemyPlayStyle[] styles = (EnemyPlayStyle[])System.Enum.GetValues(typeof(EnemyPlayStyle));
            EnemyPlayStyle chosenStyle = styles[UnityEngine.Random.Range(0, styles.Length)];

            EnemyPlayStyleSettings setting = SettingsList.Where(s => s.PlayStyle == chosenStyle).FirstOrDefault();

            return setting;
        }


        public Player GetCurrentEnemy()
        {
            return currentEnemy;
        }

        //TODO: dont create enemy instances -> fill current enemy script with new data (keep event actions alive)

        public void MakeMove(List<GameSlot> gameSlots, bool isSetter)
        {

            List<PlayCardData> cardChoices = currentEnemy.MakeMove(gameSlots, isSetter);
            
            GameManager.Instance.SetCardToSlot(cardChoices[0], PlayerSide.Enemy, SlotPosition.Position1);
            GameManager.Instance.SetCardToSlot(cardChoices[1], PlayerSide.Enemy, SlotPosition.Position2);
            GameManager.Instance.SetCardToSlot(cardChoices[2], PlayerSide.Enemy, SlotPosition.Position3);
        }
    }
}