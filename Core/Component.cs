using System;
using System.Diagnostics;
using Godot;

namespace Core
{
    public partial class Component : Node2D, IComponent
    {
        public string ComponentName { get; set; }

        public override void _EnterTree()
        {
            this.GetParent().SetMeta(this.ComponentName, this);
            GD.Print($"Component {this.ComponentName} entered tree", this);
        }

        public override void _ExitTree()
        {
            this.GetParent().RemoveMeta(this.ComponentName);
            GD.Print($"Component {this.ComponentName} exited tree");
        }

    }

    interface IComponent
    {
        string ComponentName { get; }

        void _EnterTree();
        void _ExitTree();
    }
}
