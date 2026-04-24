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
    private string resolvingText = "Resolving...";

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Button button;
    
    [SerializeField]
    private int _currentTextIndex = 0;
    [SerializeField]
    private float _currentTime = 0f;
    [SerializeField]
    private float _textTime = 0.25f;
    
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
        button.interactable = !IsResolving();
        
        
        if (IsResolving())
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _textTime)
            {
                _currentTime = 0f;
                _currentTextIndex++;
                if (_currentTextIndex >= resolvingText.Length)
                {
                    _currentTextIndex = 0;
                }
            }
            text.text = resolvingText.Substring(0, _currentTextIndex);
            return;
        }else if (IsAcceptState())
        {
            
            text.text = showResultText;
        } else
        {
            text.text = sendText;
        }
        _currentTime = 0f;
        _currentTextIndex = 0;
    }

    public void ProceedToNextStep()
    {
        GameManager.Instance.ProceedToNextState();
    }

    private bool IsAcceptState()
    {
        return GameManager.Instance.GetTurnState() == TurnState.AcceptState;
    }

    private bool IsResolving()
    {
        return GameManager.Instance.GetTurnState() == TurnState.ResolvingState;
    }

    public void HandleAnimStateChanged(bool animating)
    {
        if (button == null)
        {
            return;
        }
        
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
