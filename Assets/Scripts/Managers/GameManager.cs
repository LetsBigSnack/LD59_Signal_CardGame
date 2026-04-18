using System;
using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    public static GameManager Instance;
    
    [SerializeField]
    
    public Player Player;
    
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
        
        Player.Deck.InitializeDeck();
    }

    public void DrawCard()
    {
        Player.Deck.Draw(1);
    }
    
    public void PlayCard()
    {
        Player.Deck.PlayCard(Player.Deck.Hand[0]);
    }

    public void InitializingDecks()
    {
        
    }
    
}
