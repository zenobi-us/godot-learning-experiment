using Godot;

namespace components
{

    [GlobalClass] // Makes it available in the editor
    public partial class PositionComponent : core.BaseComponent
    {
        [Export]
        public Vector2 Position { get; set; }

    }

}
