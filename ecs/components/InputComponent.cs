using Godot;

namespace components
{
    [GlobalClass]
    public partial class InputComponent : Node, core.IComponent
    {
        [Export]
        public float Speed { get; set; }

    }
}
