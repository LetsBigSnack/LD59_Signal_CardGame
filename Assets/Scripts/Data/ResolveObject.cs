namespace Data
{
    public enum ResolveType
    {
        Attack,
        Defend,
        Interfere,
        ModPhishing,
        ModBackdoor,
        ModFirewall,
        ModBackUp,
        AttackResolve,
        DefendResolve,
        CardDraw,
        Heal
    }
    
    
    public class ResolveObject
    {
        private GameSlot _ownSlot;
        private GameSlot _oppositeSlot;
        private int _number;
        private ResolveType _resolveType;
        
        public GameSlot OwnSlot
        {
            get => _ownSlot;
            set => _ownSlot = value;
        }

        public GameSlot OppositeSlot
        {
            get => _oppositeSlot;
            set => _oppositeSlot = value;
        }

        public int Number
        {
            get => _number;
            set => _number = value;
        }

        public ResolveType ResolveType
        {
            get => _resolveType;
            set => _resolveType = value;
        }
        
        public ResolveObject(ResolveType type, int number, GameSlot ownSlot, GameSlot oppositeSlot)
        {
            this._resolveType = type;
            this._number = number;
            this._ownSlot = ownSlot;
            this._oppositeSlot = oppositeSlot;
        }
        
    }
}