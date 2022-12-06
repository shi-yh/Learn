namespace Tower_Defense
{
    public class DefenceWarEntity : GameBehavior
    {
        private DefenceWarFactory _originFactory;

        public override bool GameUpdate()
        {
            return base.GameUpdate();
        }

        public DefenceWarFactory OriginFactory
        {
            get => _originFactory;
            set { _originFactory = value; }
        }

        public void Recycle()
        {
            _originFactory.Reclaim(this);
        }
    }
}