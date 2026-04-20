using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using ScriptableObjects.BasicCards;
using ScriptableObjects.Modifier;
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
    private Dictionary<PlayerSide, int> _cardDrawModifiers = new Dictionary<PlayerSide, int>();
    
    
    [SerializeField] private int currentTurn;
    [SerializeField] private PlayerSide priority;
    [SerializeField] private int startCardAmount = 7;
    [SerializeField] private TurnState currentState = TurnState.SettingState;
    public event Action<GameSlot> OnCardRemoved;
    public event Action<GameSlot> OnCardAdded;
    
    public event Action<PlayerSide> OnPriorityChanged;

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
        Debug.Log("I HAVE RISEN");
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

        EnemyManager.Instance.CreateNewEnemy();
        
        CreateGameSlots();
        
        _cardDrawModifiers.Add(PlayerSide.Player, 0);
        _cardDrawModifiers.Add(PlayerSide.Enemy, 0);
        
        currentTurn = 0;
        priority = PlayerSide.Enemy;
        OnPriorityChanged?.Invoke(priority);
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
                    EnemyManager.Instance.MakeMove(_gameSlots, true);
                }
                //Anim 
                ProceedToNextState();
                break;
            case TurnState.RespondingState:
                if (!hasEnemyPriority)
                {
                    EnemyManager.Instance.MakeMove(_gameSlots, false);
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
        _players[PlayerSide.Player].PlayerCards.DrawUntil(startCardAmount + _cardDrawModifiers[PlayerSide.Player]);
        _players[PlayerSide.Enemy].PlayerCards.DrawUntil(startCardAmount + _cardDrawModifiers[PlayerSide.Enemy]);
        currentTurn++;
        CreateGameSlots();
        _cardDrawModifiers.Add(PlayerSide.Player, 0);
        _cardDrawModifiers.Add(PlayerSide.Enemy, 0);
        priority = priority == PlayerSide.Player ? PlayerSide.Enemy :  PlayerSide.Player;
        OnPriorityChanged?.Invoke(priority);
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
        
        //Apply Modifiers
        
        foreach (GameSlot gameSlot in stack)
        {
            
            if (gameSlot.PlayCardData == null || gameSlot.PlayCardData.Modifier == null)
            {
                continue;
            }
            
            GameSlot oppositeSlot = _gameSlots.FirstOrDefault(s =>
                s.SlotPosition == gameSlot.SlotPosition && s.PlayerSide != gameSlot.PlayerSide);
            
            gameSlot.OwnModifierType = gameSlot.PlayCardData.Modifier.ModifierType;
            if (oppositeSlot != null)
            {
                oppositeSlot.OppositeModifierType = gameSlot.PlayCardData.Modifier.ModifierType;
            }
        }
        
        foreach (GameSlot gameSlot in stack)
        {
            if (!gameSlot.IsBlocked || gameSlot.OwnModifierType == ModifierType.Encrypted)
            {
                if (gameSlot.PlayCardData == null)
                {
                    continue;
                }
                
                GameSlot oppositeSlot = _gameSlots.FirstOrDefault(s =>
                    s.SlotPosition == gameSlot.SlotPosition && s.PlayerSide != gameSlot.PlayerSide);
                
                _players[gameSlot.PlayerSide].PlayerCards.PlayCard(gameSlot.PlayCardData, gameSlot, oppositeSlot);

                if (gameSlot.PlayCardData.GetType() == typeof(InterfereCard))
                {
                    InterfereCard interfereCard = (InterfereCard)gameSlot.PlayCardData.Card;
                    if (priority == gameSlot.PlayerSide)
                    {
                        interfereCard.SetPlay(gameSlot, oppositeSlot);
                    }
                    else
                    {
                        interfereCard.RespondPlay(gameSlot, oppositeSlot);
                    }
                }
                
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

            OnCardRemoved?.Invoke(gameSlot);
        }
        
        //Draw Card
        foreach (GameSlot gameSlot in stack)
        {
            _cardDrawModifiers[gameSlot.PlayerSide] += gameSlot.CardDraw;
        }
        
        //Heal
        
        foreach (GameSlot gameSlot in stack)
        {
            if (gameSlot.Heal > 0)
            {
                _players[gameSlot.PlayerSide].HealLife(gameSlot.Heal);
            }
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

        OnCardAdded?.Invoke(gameSlot);
    
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
        
        Player current = _players[priority];
        Player opponent = _players[priority == PlayerSide.Player ? PlayerSide.Enemy : PlayerSide.Player];


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

    public Player GetOppositePlayer(PlayerSide side)
    {
        Player opponent = _players[side == PlayerSide.Player ? PlayerSide.Enemy : PlayerSide.Player];
        return opponent;
    }

}
