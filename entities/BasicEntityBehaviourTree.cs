using BehaviourTree;
using BehaviourTree.FluentBuilder;
using Godot;

namespace entities
{
    [GlobalClass]
    public partial class BasicEntityBehaviourTree : core.BehaviourTreeResource
    {

        public override IBehaviour<behaviours.BehaviourTreeBlackboardContext> GetTree()
        {
            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .PrioritySelector("Root")
                    .Subtree(behaviours.BehaviourTrees.FollowsMouseBehaviour())
                    .End()
                    .Build();

        }


    }
}
