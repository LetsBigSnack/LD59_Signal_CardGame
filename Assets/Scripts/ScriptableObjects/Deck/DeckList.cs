using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObjects.Deck
{
    [CreateAssetMenu(fileName = "DeckList", menuName = "Scriptable Objects/DeckList")]
    public class DeckList : ScriptableObject
    {
        
        public List<PlayCardData> cards;
        
    }
}
