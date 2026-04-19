using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using UnityEngine;

public enum TurnState
{
    SettingState,
    RespondingState,
    ResolvingState
}

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    
    private List<GameSlot> _gameSlots;
    private Dictionary<PlayerSide, Player>  _players = new Dictionary<PlayerSide, Player>();
    
    [SerializeField] private int currentTurn;
    [SerializeField] private PlayerSide priority;
    [SerializeField] private int startCardAmount = 7;
    [SerializeField] private TurnState currentState = TurnState.SettingState;
    
    
    public List<GameSlot> GameSlots
    {
        get => _gameSlots;
        set => _gameSlots = value;
    }

    public Dictionary<PlayerSide, Player> Players
    {
        get => _players;
        set => _players = value;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CreateGameSlots();
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    private void CreateGameSlots()
    {
        this._gameSlots = new List<GameSlot>();
        
        _gameSlots.Add(new GameSlot(SlotPosition.Position1, PlayerSide.Player));
        _gameSlots.Add(new GameSlot(SlotPosition.Position2, PlayerSide.Player));
        _gameSlots.Add(new GameSlot(SlotPosition.Position3, PlayerSide.Player));
        
        _gameSlots.Add(new GameSlot(SlotPosition.Position1, PlayerSide.Enemy));
        _gameSlots.Add(new GameSlot(SlotPosition.Position2, PlayerSide.Enemy));
        _gameSlots.Add(new GameSlot(SlotPosition.Position3, PlayerSide.Enemy));
        
    }

    public void StartGame()
    {
        CreateGameSlots();
        currentTurn = 1;
        priority = PlayerSide.Enemy;
        currentState = TurnState.SettingState;
        
        _players = new Dictionary<PlayerSide, Player>();
        _players.Add(PlayerSide.Player,  PlayerManager.Instance.Player);
        _players.Add(PlayerSide.Enemy,  EnemyManager.Instance.GetCurrentEnemy());
        
        _players[PlayerSide.Player].ResetPlayer();
        _players[PlayerSide.Enemy].ResetPlayer();
        
        _players[PlayerSide.Player].PlayerCards.DrawUntil(startCardAmount);
        _players[PlayerSide.Enemy].PlayerCards.DrawUntil(startCardAmount);
        
        HandleCurrentState();
    }


    public void HandleCurrentState()
    {
        bool hasEnemyPriority = priority == PlayerSide.Enemy;
        
        switch (currentState)
        {
            case TurnState.SettingState:
                if (hasEnemyPriority)
                {
                    EnemyManager.Instance.MakeMove();
                }
                //Anim 
                ProceedToNextState();
                break;
            case TurnState.RespondingState:
                if (!hasEnemyPriority)
                {
                    EnemyManager.Instance.MakeMove();
                }
                ProceedToNextState();
                break;
            case TurnState.ResolvingState:
                ResolveSetCards();
                ProceedToNextState();
                break;
        }
    }

    public void ProceedToNextState()
    {
        switch (currentState)
        {
            case TurnState.SettingState:
                if (_gameSlots.Where(slots => slots.PlayerSide == priority && slots.PlayCardData != null).ToList().Count != 3)
                {
                    Debug.Log("Not enough cards - Setter");
                    return;
                }
                currentState = TurnState.RespondingState;
                //Maybe Change for a nice reveal
                HandleCurrentState();
                break;
            case TurnState.RespondingState:
                if (_gameSlots.Where(slots => slots.PlayerSide != priority && slots.PlayCardData != null).ToList().Count != 3)
                {
                    Debug.Log("Not enough cards - Responder");
                    return;
                }
                currentState = TurnState.ResolvingState;
                
                //Maybe Change for a nice reveal
                HandleCurrentState();
                break;
            case TurnState.ResolvingState:
                
                RoundOutcome outcome = CheckWinner();

                switch (outcome)
                {
                    case RoundOutcome.PlayerWin:
                        WinGame();
                        break;
                    case RoundOutcome.EnemyWin:
                        LooseGame();
                        break;
                    case RoundOutcome.NoWin:
                        Debug.Log("No Win");
                        NextRound();
                        break;
                }
                break;
        }
    }

    private void NextRound()
    {
        _players[PlayerSide.Player].PlayerCards.DrawUntil(startCardAmount);
        _players[PlayerSide.Enemy].PlayerCards.DrawUntil(startCardAmount);
        currentTurn++;
        CreateGameSlots();
        priority = priority == PlayerSide.Player ? PlayerSide.Enemy : PlayerSide.Player;
        currentState = TurnState.SettingState;
    }

    private void WinGame()
    {
        
        
        GameStateManager.Instance.ChangeState(GameState.Reward);
    }

    private void LooseGame()
    {
        GameStateManager.Instance.ChangeState(GameState.Reward);
    }
    
    public void ResolveSetCards()
    {
        List<GameSlot> stack = _gameSlots
            .OrderBy(slot => slot.PlayCardData == null)
            .ThenBy(gameSlot => gameSlot.PlayCardData)
            .ThenBy(orderedGameSlot => orderedGameSlot.PlayerSide != priority)
            .ToList();
        
        //Apply Effect
        foreach (GameSlot gameSlot in stack)
        {
            //TODO: expand for the unblocked modifier
            if (!gameSlot.IsBlocked)
            {
                if (gameSlot.PlayCardData == null)
                {
                    continue;
                }
                
                GameSlot oppositeSlot = _gameSlots.FirstOrDefault(s =>
                    s.SlotPosition == gameSlot.SlotPosition && s.PlayerSide != gameSlot.PlayerSide);
                
                _players[gameSlot.PlayerSide].PlayerCards.PlayCard(gameSlot.PlayCardData, gameSlot, oppositeSlot);
                
            }
        }
        
        //Resolve Damage and Effects
        
        foreach (GameSlot gameSlot in stack)
        {
            gameSlot.ResolveTurn();
            
            
            if (gameSlot.PlayCardData == null)
            {
                continue;
            }
            
            _players[gameSlot.PlayerSide].PlayerCards.ResolveCard(gameSlot.PlayCardData);
            
        }
        
    }

    public void SetCardToSlot(PlayCardData card, PlayerSide side, SlotPosition position)
    {

        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);

        if (gameSlot == null || gameSlot.PlayCardData != null)
        {
            return;
        }
        
        
        if (_players[side].PlayerCards.SetCard(card))
        { 
            gameSlot.PlayCardData = card;
        }
        
    }
    
    public void UnsetCardToSlot(PlayCardData card, PlayerSide side, SlotPosition position)
    {

        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);

        if (gameSlot == null || gameSlot.PlayCardData == null)
        {
            return;
        }
        
        if (_players[side].PlayerCards.UnsetCard(card))
        { 
            gameSlot.PlayCardData = null;
        }
        
    }
    

    private RoundOutcome CheckWinner()
    {
        
        Player current = _players[PlayerSide.Player];
        Player opponent = _players[priority == PlayerSide.Player ? PlayerSide.Player : PlayerSide.Enemy];


        if (opponent.IsDead())
        {
            return priority == PlayerSide.Player ? RoundOutcome.PlayerWin : RoundOutcome.EnemyWin;
        }

        if (current.IsDead())
        {
            return priority == PlayerSide.Player ? RoundOutcome.EnemyWin : RoundOutcome.PlayerWin;
        }
        
        return RoundOutcome.NoWin;
    }

}
