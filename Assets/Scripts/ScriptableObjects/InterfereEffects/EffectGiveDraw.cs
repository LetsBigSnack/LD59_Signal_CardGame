using Data;
using UnityEngine;

namespace ScriptableObjects.InterfereEffects
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Effects/EffectGiveDraw")]

    public class EffectGiveDraw : InterfereEffect
    {
        
        [SerializeField]
        private int draw;
        
        public override void ApplyEffect(GameSlot ownSlot, GameSlot enemySlot)
        {
            ownSlot.AddCardDraw(draw);
        }
    }
}