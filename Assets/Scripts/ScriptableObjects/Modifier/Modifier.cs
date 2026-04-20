using UnityEngine;

namespace ScriptableObjects.Modifier
{
    public enum ModifierType
    {
        None,
        Encrypted,
        Phising,
        Backdoor,
        Firewall,
        BackUp
    }

    public enum ModifierOwnerRole
    {
        Source,
        Target
    }
    
    [CreateAssetMenu(fileName = "Modifier", menuName = "Scriptable Objects/Modifier")]
    public class Modifier : ScriptableObject
    {
        public string modName;
        public string modDescription;
        [SerializeField]
        private ModifierType modType;
        
        [SerializeField]
        private bool preventInterfere = false;
        
        public ModifierType ModifierType { get => modType; set => modType = value; }
        
        public string ModName { get => modName; set => modName = value; }
        public string ModDescription { get => modDescription; set => modDescription = value; }
        

    }
}
