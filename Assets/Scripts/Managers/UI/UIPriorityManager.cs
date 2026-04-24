using Data;
using UnityEngine;
using UnityEngine.UI;

public class UIPriorityManager : MonoBehaviour
{
    [SerializeField]
    private Sprite priority;

    [SerializeField]
    private Sprite noPriority;

    [SerializeField]
    private Image playerPrio;

    [SerializeField]
    private Image enemyPrio;

    public void OnEnable()
    {
        GameManager.Instance.OnPriorityChanged += HandlePriorityChanged;
    }

    public void OnDisable()
    {
        GameManager.Instance.OnPriorityChanged -= HandlePriorityChanged;
    }

    public void HandlePriorityChanged(PlayerSide side)
    {
        playerPrio.sprite = side == PlayerSide.Player ? priority : noPriority;
        enemyPrio.sprite = side == PlayerSide.Enemy ? priority : noPriority;
    }
}
