using Godot;

namespace core
{

    // Component interface - all components will implement this
    public interface IComponent
    {
        // Empty interface used for typing
        string Id { get; set; }
    }


    public partial class BaseComponent : Node, IComponent
    {
        public string Id { get; set; }

    }
}
