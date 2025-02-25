using Godot;
using BehaviourTree;
using BehaviourTree.FluentBuilder;

namespace behaviours
{

    public static class BehaviourTrees
    {

        public static IBehaviour<behaviours.BehaviourTreeBlackboardContext> FollowsMouseBehaviour()
        {

            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .Sequence("Find and follow the mouse")
                    .Do("Set mouse as target", ctx => BehaviourActions.SetTargetPosition(
                        ctx,
                        ctx.MousePosition
                    ))
                    .Do("Move to target", BehaviourActions.MoveToTargetPosition)
                .End()
                .Build();
        }
    }
}
