using core;
using Godot;

namespace components
{
    [GlobalClass]
    public partial class TargetPositionComponent : core.BaseComponent
    {
        /**
         * X,Y coordinates of the target position.
         */
        [Export]
        public Vector2 Position;

        /**
         * The remaining distance in which we 
         * consider the target to have been reached.
         */
        [Export]
        public int Radius;


        public TargetPositionComponent(
            string id,
            Vector2 position,
            int radius = 40
        )
        {
            Id = id;
            Position = position;
            Radius = radius;
        }
        public TargetPositionComponent(
        )
        {
        }

    }
}
