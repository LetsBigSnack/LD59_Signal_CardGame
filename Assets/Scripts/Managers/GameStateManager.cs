using System;
using UnityEngine;

namespace Managers
{

    public enum GameState
    {
        Title,
        Tutorial,
        Game,
        Pause,
        Reward,
        Remove,
        Loose
    }
    
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;
        
        [SerializeField] private GameState initialGameState;
        [SerializeField] private GameState currentGameState;

        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                currentGameState = initialGameState;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            HandleGameState();
        }

        private void HandleGameState()
        {

            switch (currentGameState)
            {
                case GameState.Title:
                    break;
                case GameState.Tutorial:
                    break;
                case GameState.Game:
                    GameManager.Instance.StartGame();
                    break;
                case GameState.Pause:
                    break;
                case GameState.Reward:
                    RewardManager.Instance.GenerateRewards();
                    break;
                case GameState.Remove:
                    break;
                case GameState.Loose:
                    break;
            }
            
        }

        public void ChangeState(GameState newState)
        {
            currentGameState = newState;
            OnGameStateChanged?.Invoke(currentGameState);
            HandleGameState();
        }
    }
}