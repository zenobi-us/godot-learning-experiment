using core;
using Godot;

namespace components
{
    [GlobalClass]
    public partial class TargetPositionComponent : Node, IComponent
    {

        [Export]
        public Vector2 Position { get; set; }

        public TargetPositionComponent(
            Vector2 position
        )
        {
            Position = position;
        }
        public TargetPositionComponent(
        )
        {
        }

    }
}
