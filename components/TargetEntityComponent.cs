using core;
using Godot;

namespace components
{
    /**
     * Entities with this component will be driven to move towards the 
     * target entity.
     *
     * This is usually intended to be a programatically attached component,
     * Typically by the TargetEntitySystem or by MoveToTargetBehaviour.
     *
     */
    [GlobalClass]
    public partial class TargetEntityComponent : Node, IComponent
    {

        [Export]
        public NodePath TargetEntityNodePath { get; set; }

        public Node TargetEntity { get; set; }

        public override void _Ready()
        {
            base._Ready();

            if (TargetEntityNodePath == null)
            {
                return;
            }

            TargetEntity = GetNode(TargetEntityNodePath);
        }

        public TargetEntityComponent(Node entity)
        {
            TargetEntity = entity;
        }
        public TargetEntityComponent()
        {
        }
    }
}
