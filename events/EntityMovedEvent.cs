// Events.cs
namespace events
{

    // Example event: EntityMoved
    public struct EntityMovedEvent : IEvent
    {
        public string EntityId { get; }
        public float OldX { get; }
        public float OldY { get; }
        public float NewX { get; }
        public float NewY { get; }

        public EntityMovedEvent(string entityId, float oldX, float oldY, float newX, float newY)
        {
            EntityId = entityId;
            OldX = oldX;
            OldY = oldY;
            NewX = newX;
            NewY = newY;
        }
    }
}
