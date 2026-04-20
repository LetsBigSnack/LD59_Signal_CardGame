using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public GameObject pauseMenu;
    public Canvas canvas;
    public GameObject mainMenuButton;
    public Camera posCam;

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

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = posCam;
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

        mainMenuButton.GetComponent<Button>().interactable = !IsStartScreen();

        if(posCam == null)
        {
            posCam = GameObject.Find("PostCam").GetComponent<Camera>();
            gameObject.GetComponent<Canvas>().worldCamera = posCam;
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

    private bool IsStartScreen()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StartScreen";
    }
}
