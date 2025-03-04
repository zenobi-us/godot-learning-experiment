using Godot;
using BehaviourTree;

namespace components
{
    [GlobalClass]
    public partial class BehaviourTreeComponent : core.BaseComponent
    {


        [Export]
        public core.BehaviourTreeResource BehaviourResource { get; set; }

        public IBehaviour<behaviours.BehaviourTreeBlackboardContext> CreateInstance()
        {
            if (BehaviourResource == null)
            {
                GD.Print("Behaviour not set");
                return null;
            }

            IBehaviour<behaviours.BehaviourTreeBlackboardContext> behaviour = BehaviourResource.GetTree();
            return behaviour;
        }

        public IBehaviour<behaviours.BehaviourTreeBlackboardContext> Behaviour { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Behaviour = CreateInstance();
            if (Behaviour == null)
            {
                GD.PrintErr("Failed to instantiate BehaviourTree from resource.");
            }
            else
            {
                GD.Print($"BehaviourTree instantiated: {Behaviour.Name}");
            }
        }

    }

}
