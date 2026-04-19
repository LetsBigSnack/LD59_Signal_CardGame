using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public GameObject pauseMenu;
    public Canvas canvas;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                CloseMenu();
                return;
            } 
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void CloseMenu()
    {
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        pauseMenu.SetActive(false);
    }
}
