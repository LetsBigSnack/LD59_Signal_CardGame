using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "AttackCard", menuName = "Scriptable Objects/Cards/AttackCard")]
    public class AttackCard : BasicCard
    {
        public int damage;
        
        public override void Play()
        {
            //TODO: add mods
            //TOOD: add effect
        }
    
    }
}
