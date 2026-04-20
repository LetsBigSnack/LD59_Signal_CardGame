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
        if (IsAcceptState())
        {
            text.text = showResultText;
        } else
        {
            text.text = sendText;
        }
    }

    public void ProceedToNextStep()
    {
        GameManager.Instance.ProceedToNextState();
    }

    public bool IsAcceptState()
    {
        return GameManager.Instance.GetTurnState() == TurnState.AcceptState;
    }
}
