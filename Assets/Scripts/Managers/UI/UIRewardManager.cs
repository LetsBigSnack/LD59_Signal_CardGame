using Data;
using Managers;
using UnityEngine;
using Unity.UI;
using System.Collections.Generic;
using UnityEngine.UI;

public enum UIRewardState
{
    Choice,
    Add,
    Remove,
    Nothing
}

public class UIRewardManager : MonoBehaviour
{
    [SerializeField]
    public List<UICard> cardSlots;

    [SerializeField]
    public GameObject ChoiceScreen;

    [SerializeField]
    public GameObject AddScreen;

    [SerializeField]
    public GameObject RemoveScreen;

    [SerializeField]
    public GameObject removeButton;

    public void OnEnable()
    {
        RewardManager.Instance.OnRewardsChanged += HandleRewardsChanged;
        GameStateManager.Instance.OnGameStateChanged += HandleTurnStateChanged;
    }

    public void OnDisable()
    {
        RewardManager.Instance.OnRewardsChanged -= HandleRewardsChanged;
        GameStateManager.Instance.OnGameStateChanged += HandleTurnStateChanged;
    }

    public void Update()
    {
        removeButton.GetComponent<Button>().interactable = IsAbleToRemove();
    }

    private bool IsAbleToRemove()
    {
        return PlayerManager.Instance.Player.PlayerCards.GetDeckList.cards.Count > PlayerCards.MinDeckCount;
    }

    public void HandleRewardsChanged(List<PlayCardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cardSlots[i].Setup(cards[i], UICardManager.Instance.GetSprite(cards[i].GetCardType()));
        }
    }

    public void HandleTurnStateChanged(GameState state)
    {
        if(state == GameState.Reward)
        {
            ChangeUiState(0);
        } else
        {
            CloseMenues();
        }
    }

    public void ChangeUiState(int intState)
    {
        UIRewardState state = (UIRewardState) intState;
        CloseMenues();
        switch (state)
        {
            case UIRewardState.Choice:
                ChoiceScreen.SetActive(true);
                break;
            case UIRewardState.Add:
                AddScreen.SetActive(true);
                break;
            case UIRewardState.Remove:
                RemoveScreen.SetActive(true);
                RemoveScreen.GetComponent<UICardCollectionView>().OpenDeck();
                break;
            case UIRewardState.Nothing:
                RewardManager.Instance.SkipReward();
                break;
        }
    }

    public void CloseMenues()
    {
        ChoiceScreen.SetActive(false);
        AddScreen.SetActive(false);
        RemoveScreen.SetActive(false);
    }
}
