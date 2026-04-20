using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGivePrio")]
    public class EffectGivePrio : InterfereEffect
    {
        
        [SerializeField]
        private int prioTurns;
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddPrio(prioTurns);
        }
    }
}