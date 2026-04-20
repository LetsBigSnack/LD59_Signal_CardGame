using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGetHeal")]
    public class EffectGetHeal : InterfereEffect
    {
        
        [SerializeField]
        private int heal;
        public override string GetEffectDescription()
        {
            return base.GetEffectDescription().Replace("{x}",  Mathf.Abs(heal).ToString());
        }
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            ownSlot.AddHeal(heal);
        }
    }
}