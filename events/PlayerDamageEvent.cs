namespace events
{
    public struct PlayerDamagedEvent : IEvent
    {
        public int Damage { get; }
        public PlayerDamagedEvent(int damage) => Damage = damage;
    }
}
