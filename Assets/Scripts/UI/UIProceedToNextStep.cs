using UnityEngine;
using Data;
using TMPro;
using UnityEngine.UI;

public class UIProceedToNextStep : MonoBehaviour
{
    [SerializeField]
    private string sendText;

    [SerializeField]
    private string showResultText;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Button button;

    public void Start()
    {
        UISlotManager.Instance.OnAnimStateChanged += HandleAnimStateChanged;
    }

    public void OnDisable()
    {
        UISlotManager.Instance.OnAnimStateChanged -= HandleAnimStateChanged;
    }

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

    public void HandleAnimStateChanged(bool animating)
    {
        if (animating)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
