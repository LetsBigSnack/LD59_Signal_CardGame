using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGivePrio")]
    public class EffectGivePrio : InterfereEffect
    {
        
        [SerializeField]
        private int prioTurns;
        
        public override string GetEffectDescription()
        {
            return base.GetEffectDescription().Replace("{x}", prioTurns.ToString());
        }
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddPrio(prioTurns);
        }
    }
}