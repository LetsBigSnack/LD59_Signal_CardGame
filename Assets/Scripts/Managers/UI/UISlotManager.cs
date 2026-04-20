using Data;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using JetBrains.Annotations;

public class UISlotManager : MonoBehaviour
{
    public static UISlotManager Instance;

    [SerializeField]
    private UISlot[] playerSlots;
    [SerializeField]
    private UISlot[] enemySlots;
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private List<GameSlot> enemyCardQueue;

    private Coroutine animCoroutine;

    public event Action<bool> OnAnimStateChanged;

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

    private void OnEnable()
    {
        gameManager.OnCardRemoved += HandleCardRemoved;
        gameManager.OnCardAdded += HandleCardAdded;
    }

    private void OnDisable()
    {
        gameManager.OnCardRemoved -= HandleCardRemoved;
        gameManager.OnCardAdded -= HandleCardAdded;
    }

    private void HandleCardAdded(GameSlot gameSlot)
    {
        if (gameSlot.PlayerSide == PlayerSide.Enemy)
        {
            enemyCardQueue.Add(gameSlot);
            if (enemyCardQueue.Count == 3)
            {
                if (animCoroutine == null)
                {
                    animCoroutine = StartCoroutine(PlaySlotAnimations());
                }

            }
        }
        else
        {
            GetSlot(gameSlot).SetCard(gameSlot.PlayCardData);
        }   
    }
    private void HandleCardRemoved(GameSlot gameSlot)
    {
        GetSlot(gameSlot).ClearSlot();
    }

    private UISlot GetSlot(GameSlot gameSlot)
    {
        return gameSlot.PlayerSide == PlayerSide.Player ? playerSlots[(int)gameSlot.SlotPosition] : enemySlots[(int)gameSlot.SlotPosition];
    }

    private IEnumerator PlaySlotAnimations()
    {
        OnAnimStateChanged?.Invoke(true);
        for (int i = 0;  i < enemyCardQueue.Count; i++)
        {
            GetSlot(enemyCardQueue[i]).SetCard(enemyCardQueue[i].PlayCardData);
            yield return new WaitForSeconds(0.3f);
        }
        enemyCardQueue.Clear();
        animCoroutine = null;
   
        GameManager.Instance.ProceedToNextState();
        OnAnimStateChanged?.Invoke(false);
    }
}
