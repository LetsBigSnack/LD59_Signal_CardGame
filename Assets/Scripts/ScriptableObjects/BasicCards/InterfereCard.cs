using Data;
using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Cards/InterfereCard")]
    public class InterfereCard : BasicCard
    {
        public int priority;
        
        public override void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.BlockSlot();
        }
    
    }
}
