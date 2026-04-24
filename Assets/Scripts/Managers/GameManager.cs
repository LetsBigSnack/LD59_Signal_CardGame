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
    AcceptState,
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
    public event Action<ResolveObject> OnResolve;
    public event Action<bool> OnResolveFinished;
    
    public event Action<PlayerSide> OnPriorityChanged;
    
    [SerializeField] private bool allowPartial = false;
    
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

    public PlayerSide GetPriority()
    {
        return priority;
    }

    public TurnState GetTurnState()
    {
        return currentState;
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

        foreach (GameSlot slot in _gameSlots)
        {
            OnCardRemoved?.Invoke(slot);
        }
        
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        OnResolveFinished?.Invoke(false);
        EnemyManager.Instance.CreateNewEnemy();
        
        CreateGameSlots();
        _cardDrawModifiers = new Dictionary<PlayerSide, int>();
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
                    return;
                }
                ProceedToNextState();
                break;
            case TurnState.RespondingState:
                if (!hasEnemyPriority)
                {
                    EnemyManager.Instance.MakeMove(_gameSlots, false);
                    return;
                }
                //ProceedToNextState();
                break;
            case TurnState.AcceptState:
                if (priority != PlayerSide.Player)
                {
                    ProceedToNextState();
                }
                break;
            case TurnState.ResolvingState:
                ResolveSetCards();
                break;
        }
    }

    public void ProceedToNextState()
    {
        switch (currentState)
        {
            case TurnState.SettingState:
                if (!allowPartial && _gameSlots.Where(slots => slots.PlayerSide == priority && slots.PlayCardData != null).ToList().Count != 3)
                {
                    Debug.Log("Not enough cards - Setter");
                    return;
                }
                currentState = TurnState.RespondingState;
                //Maybe Change for a nice reveal
                HandleCurrentState();
                break;
            case TurnState.RespondingState:
                if (!allowPartial & _gameSlots.Where(slots => slots.PlayerSide != priority && slots.PlayCardData != null).ToList().Count != 3)
                {
                    Debug.Log("Not enough cards - Responder");
                    return;
                }
                currentState = TurnState.AcceptState;
                if (priority != PlayerSide.Player)
                {
                    HandleCurrentState();
                }
                break;
            case TurnState.AcceptState:
                
                currentState = TurnState.ResolvingState;
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

    public void NextRound()
    {
        OnResolveFinished?.Invoke(false);
        _players[PlayerSide.Player].PlayerCards.DrawUntil(startCardAmount + _cardDrawModifiers[PlayerSide.Player]);
        _players[PlayerSide.Enemy].PlayerCards.DrawUntil(startCardAmount + _cardDrawModifiers[PlayerSide.Enemy]);
        currentTurn++;
        CreateGameSlots();
        _cardDrawModifiers[PlayerSide.Enemy] = 0;
        _cardDrawModifiers[PlayerSide.Player] = 0;
        priority = priority == PlayerSide.Player ? PlayerSide.Enemy :  PlayerSide.Player;
        OnPriorityChanged?.Invoke(priority);
        currentState = TurnState.SettingState;
        HandleCurrentState();
    }

    private void WinGame()
    {
        GameStateManager.Instance.ChangeState(GameState.Reward);
    }

    private void LooseGame()
    {
        GameStateManager.Instance.ChangeState(GameState.Loose);
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
        
        //Stack Resolve
        
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
                
                gameSlot.OppositeGameSlot = oppositeSlot;
                
                _players[gameSlot.PlayerSide].PlayerCards.PlayCard(gameSlot.PlayCardData, gameSlot, oppositeSlot);

                if (gameSlot.PlayCardData.Card.GetType() == typeof(InterfereCard))
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
        }

        //Discard Hand
        _players[PlayerSide.Player].PlayerCards.DiscardHand();
        _players[PlayerSide.Enemy].PlayerCards.DiscardHand();
        
        //Heal
        
        foreach (GameSlot gameSlot in stack)
        {
            if (gameSlot.Heal > 0)
            {
                //_players[gameSlot.PlayerSide].HealLife(gameSlot.Heal);
                AddToResolveQueue(new ResolveObject(ResolveType.Heal, gameSlot.Heal, gameSlot, gameSlot.OppositeGameSlot));
            }
        }
        
        //Draw Card
        foreach (GameSlot gameSlot in stack)
        {
            if (gameSlot.CardDraw <= 0)
            {
                continue;
            }
            _cardDrawModifiers[gameSlot.PlayerSide] += gameSlot.CardDraw;
            AddToResolveQueue(new ResolveObject(ResolveType.CardDraw, gameSlot.CardDraw, gameSlot, gameSlot.OppositeGameSlot));
        }
        
        OnResolveFinished?.Invoke(true);
        
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
    
    //TODO: remove form Card Slot
    public bool UnsetCardToSlot(PlayerSide side, SlotPosition position)
    {
        if (!CanUnset(side, position))
        {
            return false;
        }
        
        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);
        
        if (_players[side].PlayerCards.UnsetCard(gameSlot.PlayCardData))
        {
            OnCardRemoved?.Invoke(gameSlot);
            gameSlot.PlayCardData = null;
            return true;
        }
        return false;
    }

    public bool CanUnset(PlayerSide side, SlotPosition position)
    {
        if (currentState == TurnState.AcceptState || currentState == TurnState.ResolvingState)
        {
            return false;
        }
        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);

        if (gameSlot == null || gameSlot.PlayCardData == null)
        {
            return false;
        }
        
        return true;

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

    public void AddToResolveQueue(ResolveObject resolveObject)
    {
        OnResolve?.Invoke(resolveObject);
    }

    public PlayCardData GetCardOnSlot(PlayerSide playerSide, SlotPosition slotPosition)
    {
        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == slotPosition && slot.PlayerSide == playerSide);
        if (gameSlot == null || gameSlot.PlayCardData == null)
        {
            return null;
        }
        return gameSlot.PlayCardData;
    }
}
