using System;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.BasicCards;
using ScriptableObjects.Modifier;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PlayCardData
    {
        [SerializeField]
        private BasicCard card;
        
        [SerializeField]
        private List<Modifier> modifiers;


        public string GetCardName()
        {
            //Add Mods
            return card.GetCardName();
        }
        
        public string GetCardDescription()
        {
            //Add Mods
            return card.GetCardDescription();
        }
        
        public void PlayCard()
        {
            //Add Mods
            card.Play();
        }
    }
}