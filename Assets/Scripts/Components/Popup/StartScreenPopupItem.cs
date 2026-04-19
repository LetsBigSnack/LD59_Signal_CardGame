using UnityEngine;

public class StartScreenPopupItem : MonoBehaviour
{
    [Header("Destroy Time")]
    public int destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
