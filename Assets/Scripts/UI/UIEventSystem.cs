using UnityEngine;

public class UIEventSystem : MonoBehaviour
{
    public static UIEventSystem Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
     }
}
