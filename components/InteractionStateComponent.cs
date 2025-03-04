using Godot;

namespace components
{

    [GlobalClass]
    public partial class InteractionStateComponent : core.BaseComponent
    {
        [Export]
        public NodePath[] Transitions { get; set; } = new NodePath[0]; // Paths to possible next states

        // Called when this state becomes active
        public virtual void OnEnter()
        {
            GD.Print($"Entered state: {Name}");
        }

        // Called when this state is active, returns the next state or null to stay
        public virtual Node ProcessInput(InputEvent @event)
        {
            return null; // Override in derived states
        }

        public virtual void OnExit()
        {
            GD.Print($"Exited state: {Name}");
        }
    }
}
