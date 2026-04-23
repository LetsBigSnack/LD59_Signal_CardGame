using Data;
using UnityEngine;
using ScriptableObjects.InterfereEffects;
namespace ScriptableObjects.BasicCards
{
    [CreateAssetMenu(fileName = "InterfereCard", menuName = "Scriptable Objects/Cards/InterfereCard")]
    public class InterfereCard : BasicCard
    {
        public int priority;
        
        [SerializeField]
        public InterfereEffect SetEffect;
        [SerializeField]
        public InterfereEffect RespondEffect;
        
        
        
        public override void Play(GameSlot ownSlot, GameSlot enemySlot)
        {
            enemySlot.BlockSlot();
        }
        
        public override string GetCardDescription()
        {
            string description = base.GetCardDescription() + "\n" +
                                 "Send: " + SetEffect.GetEffectDescription() + "\n" +
                                 "Receive: " + RespondEffect.GetEffectDescription() + "\n";
            
            return description;
        }

        public virtual void SetPlay(GameSlot ownSlot, GameSlot opponent)
        {
            SetEffect.ApplyEffect(ownSlot, opponent);
        }

        public virtual void RespondPlay(GameSlot ownSlot, GameSlot opponent)
        {
            RespondEffect.ApplyEffect(ownSlot, opponent);
            
        }
    }
}
