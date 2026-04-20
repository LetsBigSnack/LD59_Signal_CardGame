using System;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBarElement : MonoBehaviour
{
    [Header("StartColor")]
    public Color color;
    private Color empty = new Color32(6, 22, 15, 255);

    public void setEmpty(bool isEmpty)
    {
        Image image = gameObject.GetComponent<Image>();
        image.color = isEmpty? empty : color;
    }
}
