using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "DefenseCard", menuName = "Scriptable Objects/Cards/DefenseCard")]
    public class DefenseCard : BasicCard
    {
        public int defense;
        
        public override void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            ownSlot.AddDefense(defense);
        }
    
    }
}
