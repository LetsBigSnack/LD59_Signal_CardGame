using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGetPrio")]
    public class EffectGetPrio : InterfereEffect
    {
        
        [SerializeField]
        private int prioTurns;
        
        public override string GetEffectDescription()
        {
            return base.GetEffectDescription().Replace("{x}", Mathf.Abs(prioTurns).ToString());
        }
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            ownSlot.AddPrio(prioTurns);
        }
    }
}