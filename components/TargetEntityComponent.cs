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
    public partial class TargetEntityComponent : core.BaseComponent
    {
        /**
         * The Editor UI NodePath to our target entity
         */
        [Export]
        public NodePath TargetEntityNodePath { get; set; }


        /**
         * The remaining distance in which we consider our 
         * target to have been reached.
         */
        [Export]
        public int Threshold { get; set; } = 40;

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

        public TargetEntityComponent(Node entity, int threshold = 40)
        {
            TargetEntity = entity;
            Threshold = threshold;
        }
        public TargetEntityComponent(Node entity, string entityId, int threshold = 40)
        {
            TargetEntity = entity;
            Id = entityId;
            Threshold = threshold;
        }
        public TargetEntityComponent()
        {
        }
    }
}
