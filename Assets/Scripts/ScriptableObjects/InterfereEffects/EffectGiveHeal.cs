using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGiveHeal")]
    public class EffectGiveHeal : InterfereEffect
    {
        
        [SerializeField]
        private int heal;
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddHeal(heal);
        }
    }
}