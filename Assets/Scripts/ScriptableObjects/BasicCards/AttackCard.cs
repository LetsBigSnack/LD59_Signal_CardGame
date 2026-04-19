using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "AttackCard", menuName = "Scriptable Objects/Cards/AttackCard")]
    public class AttackCard : BasicCard
    {
        public int damage;
        
        public override void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.AddAttack(damage);
        }
    
    }
}
