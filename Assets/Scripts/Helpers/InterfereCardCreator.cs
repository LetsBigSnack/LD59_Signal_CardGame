using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects.InterfereEffects;

[CreateAssetMenu(fileName = "InterfereCardCreator", menuName = "Tools/Interfere Card Creator")]
public class InterfereCardCreator : ScriptableObject
{
    public List<InterfereEffect> setEffects = new();
    public List<InterfereEffect> respondEffects = new();
}