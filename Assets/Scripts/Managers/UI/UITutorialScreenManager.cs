using Managers;
using UnityEngine;

public class UITutorialScreenManager : MonoBehaviour
{
    [SerializeField]
    public int currentTutInt;

    [SerializeField]
    public GameObject tut1;

    [SerializeField]
    public GameObject tut2;

    [SerializeField]
    public GameObject tut3;

    [SerializeField]
    public GameObject buttonPrevious;

    [SerializeField]
    public GameObject buttonNext;

    [SerializeField]
    public GameObject buttonStart;


    public void PreviousPage()
    {
        if (currentTutInt == 0) return;
        currentTutInt--;
        EnableTut();
    }

    public void NextPage()
    {
        if(currentTutInt == 2) return;
        currentTutInt++;
        EnableTut();
    }

    private void Update()
    {
        if(currentTutInt == 2)
        {
            buttonStart.SetActive(true);
            buttonNext.SetActive(false);
        }
        else

        {
            buttonStart.SetActive(false);
            buttonNext.SetActive(true);
        }

        if(currentTutInt == 0)
        {
            buttonPrevious.SetActive(false);
        }
        else
        {
            buttonPrevious.SetActive(true);
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting game");
        Destroy(gameObject);
        GameStateManager.Instance.ChangeState(GameState.Game);
    }

    public void CloseTuts()
    {
        tut1.SetActive(false);
        tut2.SetActive(false);
        tut3.SetActive(false);
    }

    public void EnableTut()
    {
        CloseTuts();
        switch (currentTutInt)
        {
            case 0:
                tut1.SetActive(true);
                break;
            case 1:
                tut2.SetActive(true);
                break;
            case 2:
                tut3.SetActive(true);
                break;
        }
    }
}
