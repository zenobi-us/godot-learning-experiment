using System;
using Godot;

namespace components
{
    [GlobalClass]
    public partial class RenderableComponent : Node
    {
        [Export]
        public NodePath RenderNodePath { get; set; }

        public AnimatedSprite2D RenderNode { get; private set; }

        public StringName _animation { get; set; }

        public override void _Ready()
        {
            if (RenderNodePath == null)
            {
                throw new Exception("RenderNodePath is not set");
            }

            RenderNode = GetNodeOrNull<AnimatedSprite2D>(RenderNodePath);
            if (RenderNode == null)
            {
                GD.PrintErr($"Invalid RenderNodePath: {RenderNodePath} in {GetParent().Name}/Renderable");
            }
        }
    }
}
