using UnityEngine;

namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Cards/InterfereCard")]
    public class InterfereCard : BasicCard
    {
        public int priority;
        
        public override void Play()
        {
            //TODO: add mods
            //TOOD: add effect
        }
    
    }
}
