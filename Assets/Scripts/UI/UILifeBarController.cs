using UnityEngine;
using System.Collections.Generic;

public class UILifeBarController : MonoBehaviour
{
    [Header("LifeBarElements")]
    public List<UILifeBarElement> lifeBarElements;

    public void SetCurrentHealth(int amount)
    { 
        for (int i = 0; i < lifeBarElements.Count - amount; i++)
        {
            lifeBarElements[i].setEmpty(true);
        }

        for(int i = lifeBarElements.Count - amount; i < lifeBarElements.Count; i++)
        {
            lifeBarElements[i].setEmpty(false);
        }
    }
}
