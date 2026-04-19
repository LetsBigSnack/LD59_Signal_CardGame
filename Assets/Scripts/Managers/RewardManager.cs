using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Managers
{
    public class RewardManager : MonoBehaviour
    {
        public static RewardManager Instance;


        [SerializeField] private bool hasReward = false;
        [SerializeField] private bool isRemoveView = false;
        [SerializeField] private List<PlayCardData> currentRewards;
        [SerializeField] private int rewardAmount = 3;

        public List<PlayCardData> CurrentRewards
        {
            get => currentRewards;
        }

        public bool HasRewards
        {
            get => hasReward;
        }

        public bool IsRemoveView
        {
            get => isRemoveView;
            set => isRemoveView = value;
        }
        
        
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

        public void GenerateRewards()
        {
            for (int i = 0; i < rewardAmount; i++)
            {
                currentRewards.Add(CardManager.Instance.GetRandomPlayCard());
            }
            hasReward = true;
        }

        public void SkipReward()
        {
            CleanRewards();
            hasReward = false;
            GameStateManager.Instance.ChangeState(GameState.Remove);
        }
        
        public void SelectReward(PlayCardData reward)
        {
            if (!currentRewards.Contains(reward))
            {
                return;
            }
            hasReward = false;
            PlayerManager.Instance.Player.PlayerCards.AddToDeck(reward);
            CleanRewards();
            GameStateManager.Instance.ChangeState(GameState.Game);
        }

        private void CleanRewards()
        {
            currentRewards = new List<PlayCardData>();
        }
        
        public void ShowRemoveCard()
        {
            if (PlayerManager.Instance.Player.PlayerCards.GetDeckList.cards.Count <= PlayerCards.MinDeckCount)
            {
                return;
            }
            
            hasReward = false;
            CleanRewards();
            isRemoveView = true;
            GameStateManager.Instance.ChangeState(GameState.Remove);
        }
        
    }
}
