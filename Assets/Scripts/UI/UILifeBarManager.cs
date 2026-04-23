using UnityEngine;
using Data;
using Managers;

public class UILifeBarManager : MonoBehaviour
{
    [Header("LifeBarEnemy")]
    public UILifeBarController lifeBarEnemy;

    [Header("LifeBarPlayer")]
    public UILifeBarController lifeBarPlayer;

    private void OnEnable()
    {
        PlayerManager.Instance.Player.OnLifeChanged += HandleLifeChanged;
        EnemyManager.Instance.GetCurrentEnemy().OnLifeChanged += HandleLifeChanged;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.Player.OnLifeChanged -= HandleLifeChanged;
        EnemyManager.Instance.GetCurrentEnemy().OnLifeChanged -= HandleLifeChanged;
    }

    private void HandleLifeChanged(PlayerSide side, int health)
    {
        if (side == PlayerSide.Player)
        {
            lifeBarPlayer.SetCurrentHealth(health);
            Debug.Log(side + "|" + health);
        }
        else
        {
            lifeBarEnemy.SetCurrentHealth(health);
            Debug.Log(side + "|" + health);
        }            
    }
}
