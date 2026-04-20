using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGetPrio")]
    public class EffectGetPrio : InterfereEffect
    {
        
        [SerializeField]
        private int prioTurns;
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            ownSlot.AddPrio(prioTurns);
        }
    }
}