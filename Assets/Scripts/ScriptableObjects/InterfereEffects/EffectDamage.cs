using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectDamage")]
    public class EffectDamage : InterfereEffect
    {
        
        [SerializeField]
        private int damage;
        
        public override string GetEffectDescription()
        {
            return base.GetEffectDescription().Replace("{x}", Mathf.Abs(damage).ToString());
        }
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddAttack(damage);
        }
    }
}