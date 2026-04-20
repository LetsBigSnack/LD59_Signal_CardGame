using UnityEngine;
using UnityEngine.UI;

public class UIFixSceneButtons : MonoBehaviour
{
    public Button startButton;
    public Button endButton;

    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.Instance.LoadScene("FinalGameScene");
        });

        endButton.onClick.AddListener(() =>
        {
            SceneManager.Instance.EndGame();
        });
    }
}
