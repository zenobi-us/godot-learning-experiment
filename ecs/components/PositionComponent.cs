using Godot;

namespace components
{

    [GlobalClass] // Makes it available in the editor
    public partial class PositionComponent : Node, core.IComponent
    {
        [Export]
        public Vector2 Position { get; set; }

    }

}
