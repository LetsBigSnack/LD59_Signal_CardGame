using UnityEngine;
using Data;
using TMPro;

public class UIProceedToNextStep : MonoBehaviour
{
    [SerializeField]
    private string sendText;

    [SerializeField]
    private string showResultText;

    [SerializeField]
    private TextMeshProUGUI text;

    private void Update()
    {
        if (IsEnemyResolvingState())
        {
            text.text = showResultText;
        } else
        {
            text.text = sendText;
        }
    }

    public void ProceedToNextStep()
    {
        if(IsEnemyResolvingState())
        {
            GameManager.Instance.NextRound();
        } else
        {
            GameManager.Instance.ProceedToNextState();
        }
    }

    public bool IsEnemyResolvingState()
    {
        return GameManager.Instance.GetPriority() == PlayerSide.Player && GameManager.Instance.GetTurnState() == TurnState.ResolvingState;
    }
}
