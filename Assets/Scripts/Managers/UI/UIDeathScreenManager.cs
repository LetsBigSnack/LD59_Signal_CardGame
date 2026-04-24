using UnityEngine;
using Data;
using Managers;

public class UIDeathScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject deathScreen;

    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= HandleStateChanged;
    }

    public void HandleStateChanged(GameState state)
    {
        if (state == GameState.Loose)
        {
            this.deathScreen.SetActive(true);
        }
        else
        {
            this.deathScreen.SetActive(false);
        }
    }

    public void Continue()
    {
        GameStateManager.Instance.ChangeState(GameState.Game);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
    }
}
