using Godot;
using BehaviourTree;
using BehaviourTree.FluentBuilder;
using components;

namespace behaviours
{

    public static class BehaviourTrees
    {

        public static IBehaviour<behaviours.BehaviourTreeBlackboardContext> FollowsMouseBehaviour()
        {

            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .Sequence("Follow the mouse")
                        .Do("Set mouse as target", ctx => BehaviourActions.SetTargetPosition(
                            ctx,
                            ctx.MousePosition
                        ))
                            .Condition("Is mouse out of reach?", ctx =>
                            {
                                TargetPositionComponent target = ctx.EntityManager.GetComponent<TargetPositionComponent>(ctx.Entity);
                                PositionComponent position = ctx.EntityManager.GetComponent<PositionComponent>(ctx.Entity);
                                if (position == null || target == null)
                                {
                                    return false;
                                }
                                var distance = position.Position.DistanceTo(target.Position);
                                return distance > 100;
                            })
                            .Do("Move to target", BehaviourActions.MoveToTargetPosition)
                .End()
                .Build();
        }
    }
}
