using core;
using Godot;

namespace components
{
    /**
     *
     */
    public class Waypoint
    {
        public string Id;
        public string Name;
        public Vector2 Position;

        public int Radius;

        public bool Reached;
        
        public Waypoint(string name, Vector2 position, int radius = 40)
        {
            Name = name;
            Position = position;
            Radius = radius;
            Reached = false;
        }
    }

    public partial class TargetWaypointsComponent : BaseComponent
    {
        public Waypoint[] Waypoints;
    }
}
