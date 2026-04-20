using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "AttackCard", menuName = "Scriptable Objects/Cards/AttackCard")]
    public class AttackCard : BasicCard
    {
        public int damage;

        public override string GetCardDescription()
        {
            return base.GetCardDescription().Replace("{x}", damage.ToString());
        }
        
        public override void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddAttack(damage);
        }
    
    }
}
