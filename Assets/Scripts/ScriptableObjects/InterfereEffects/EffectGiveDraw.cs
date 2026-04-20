using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGiveDraw")]

    public class EffectGiveDraw : InterfereEffect
    {
        
        [SerializeField]
        private int draw;
        
        public override string GetEffectDescription()
        {
            return base.GetEffectDescription().Replace("{x}", Mathf.Abs(draw).ToString());
        }
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddCardDraw(draw);
        }
    }
}