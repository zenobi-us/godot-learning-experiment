using Godot;

namespace components
{
    [GlobalClass]
    public partial class NameableComponent : core.BaseComponent
    {
        [Export]
        public new string Id { get; set; }

        public NameableComponent(string id)
        {
            this.Id = id;
        }
    }
}
