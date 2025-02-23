using Godot;

namespace components
{
    [GlobalClass]
    public partial class VelocityComponent : Node, core.IComponent
    {
        /**
        * The acceleration applied to the moveable object when moving
        * horizontally. This controls how quickly the object accelerates as it moves left or right.
        */
        [Export]
        public double ACCELERATION = 900;

        [Export]
        public double MAX_SPEED = 120;

        /**
        * The minimum speed at which the moveable object can move.
        * This helps prevent the object from moving too slowly, especially
        * when it's stationary or near the ground.
        */
        [Export]
        public double MIN_SPEED = 15;

        /**
        * The multiplier applied to the speed when moving in air.
        * This is useful for creating a more realistic feel of movement
        * in games where gravity plays a significant role.
        */
        [Export]
        public Vector2 Velocity { get; set; }

    }
}
