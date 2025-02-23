// InteractionComponentNode.cs
using Godot;

namespace components
{

    [GlobalClass]
    public partial class InteractionComponent : Node
    {
        [Export]
        public NodePath EntryPointState { get; set; } // Path to the initial state node

        [Export]
        public bool IsActive { get; set; } = false; // Whether the interaction is currently active

        public Node CurrentState { get; set; } // Tracks the active state node

        public override void _Ready()
        {
            if (EntryPointState == null)
            {
                GD.PrintErr("Error: EntryPointState is not set.");
                IsActive = false;
                return;
            }
            CurrentState = GetNode(EntryPointState);
        }
    }

}
