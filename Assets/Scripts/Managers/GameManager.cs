using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    public static GameManager Instance;
    
    
    [SerializeField]
    private Player player;
    
    [SerializeField]
    private Player enemy;
    
    private List<GameSlot> _gameSlots;


    [SerializeField] private int currentTurn;
    [SerializeField] private PlayerSide priority;
    [SerializeField] private int startCardAmount = 7;


    public Player Player
    {
        get => player;
        set => player = value;
    }
    
    public Player Enemy
    {
        get => enemy;
        set => enemy = value;
    }

    public List<GameSlot> GameSlots
    {
        get => _gameSlots;
        set => _gameSlots = value;
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
        
        _gameSlots.Add(new GameSlot(SlotPosition.Position1, PlayerSide.Player1));
        _gameSlots.Add(new GameSlot(SlotPosition.Position2, PlayerSide.Player1));
        _gameSlots.Add(new GameSlot(SlotPosition.Position3, PlayerSide.Player1));
        
        _gameSlots.Add(new GameSlot(SlotPosition.Position1, PlayerSide.Player2));
        _gameSlots.Add(new GameSlot(SlotPosition.Position2, PlayerSide.Player2));
        _gameSlots.Add(new GameSlot(SlotPosition.Position3, PlayerSide.Player2));
        
    }

    public void StartGame()
    {
        CreateGameSlots();
        currentTurn = 0;
        priority = PlayerSide.Player1;
        
        player.ResetPlayer();
        enemy.ResetPlayer();
        
        player.PlayerCards.Draw(startCardAmount);
        enemy.PlayerCards.Draw(startCardAmount);
    }

    public void SetCardToSlot(PlayCardData card, PlayerSide side, SlotPosition position)
    {

        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);

        if (gameSlot == null || gameSlot.PlayCardData != null)
        {
            return;
        }
        
        switch (side)
        {
            case PlayerSide.Player1:
                //Check if Slot is Empty
                if (player.PlayerCards.SetCard(card))
                {
                    gameSlot.PlayCardData = card;
                }
                break;
            case PlayerSide.Player2:
                if (enemy.PlayerCards.SetCard(card))
                {
                    gameSlot.PlayCardData = card;
                }
                break;
            default:
                break;
        }
    }
    
    public void UnsetCardToSlot(PlayCardData card, PlayerSide side, SlotPosition position)
    {

        GameSlot gameSlot = _gameSlots.FirstOrDefault(slot => slot.SlotPosition == position && slot.PlayerSide == side);

        if (gameSlot == null || gameSlot.PlayCardData == null)
        {
            return;
        }
        
        switch (side)
        {
            case PlayerSide.Player1:
                //Check if Slot is Empty
                if (player.PlayerCards.UnsetCard(card))
                {
                    gameSlot.PlayCardData = null;
                }
                break;
            case PlayerSide.Player2:
                if (enemy.PlayerCards.UnsetCard(card))
                {
                    gameSlot.PlayCardData = null;
                }
                break;
            default:
                break;
        }
    }
    
    public void DrawCard()
    {
        player.PlayerCards.Draw(1);
    }
    
    public void PlayCard()
    {
        player.PlayerCards.PlayCard(player.PlayerCards.Hand[0]);
    }
    
}
