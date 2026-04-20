using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    public class InterfereEffect :ScriptableObject
    {
        [SerializeField]
        private string effectDescription;

        public int tier;
        
        public virtual string GetEffectDescription()
        {
            return effectDescription;
        }
        
        public virtual void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            
        }
    }
}