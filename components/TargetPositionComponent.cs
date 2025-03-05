using core;
using Godot;

namespace components
{
    [GlobalClass]
    public partial class TargetPositionComponent : core.BaseComponent
    {

        [Export]
        public Vector2 Position { get; set; }


        public TargetPositionComponent(
            string id,
            Vector2 position
        )
        {
            Id = id;
            Position = position;
        }
        public TargetPositionComponent(
        )
        {
        }

    }
}
