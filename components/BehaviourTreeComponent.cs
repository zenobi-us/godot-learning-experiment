using Godot;
using BehaviourTree;
using System.Reflection;
using System.Linq;

namespace components
{
    [GlobalClass]
    public partial class BehaviourTreeComponent : Node, core.IComponent
    {
        [Export]
        public core.BehaviourTreeResource BehaviourResource { get; set; }

        public BehaviourTree.IBehaviour<behaviours.BehaviourTreeBlackboardContext> CreateInstance()
        {
            if (BehaviourResource == null)
            {
                GD.Print("Behaviour not set");
                return null;
            }

            BehaviourTree.IBehaviour<behaviours.BehaviourTreeBlackboardContext> behaviour = BehaviourResource.GetTree();
            GD.Print($"BehaviourTree type: {BehaviourTree?.GetType().Name ?? "null"}");

            return behaviour;

        }

        public IBehaviour<behaviours.BehaviourTreeBlackboardContext> BehaviourTree { get; set; }

        public override void _Ready()
        {
            base._Ready();
            BehaviourTree = CreateInstance();
            if (BehaviourTree == null)
            {
                GD.PrintErr("Failed to instantiate BehaviourTree from resource.");
            }
            else
            {
                GD.Print($"BehaviourTree instantiated: {BehaviourTree.GetType().Name}");
            }
        }

    }

}
