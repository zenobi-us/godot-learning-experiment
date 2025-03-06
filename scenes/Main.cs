// Main.cs (Updated)
using Godot;

public partial class Main : Node
{
    private core.EntityManager _entityManager;
    private core.EventManager _eventManager;

    public override void _Ready()
    {
        _eventManager = GetNode<core.EventManager>("EventManager");

        // Subscribe to EntityMoved event for debugging
        _eventManager.Subscribe<events.EntityMovedEvent>(OnEntityMoved);
    }

    private void OnEntityMoved(events.EntityMovedEvent evt)
    {
        // GD.Print($"Entity {evt.EntityId} moved from ({evt.OldX}, {evt.OldY}) to ({evt.NewX}, {evt.NewY})");
    }
}
