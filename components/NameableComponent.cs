using Godot;

namespace components
{
    [GlobalClass]
    public partial class NameableComponent : core.BaseComponent
    {
        [Export]
        public new string Name { get; set; }
    }
}
