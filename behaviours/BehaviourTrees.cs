using Godot;
using BehaviourTree;
using BehaviourTree.FluentBuilder;
using components;
using System;
using BehaviourTree.Composites;

namespace behaviours
{

    public static class BehaviourTrees
    {

        public static IBehaviour<behaviours.BehaviourTreeBlackboardContext> FollowsMouseBehaviour()
        {
            var userMousePositionId = "User Mouse";
            var patrolPositionId = "Patrol";

            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .Sequence("Follow the mouse")
                    .Do("Set mouse as target", BehaviourActions.SetTargetPosition(
                        userMousePositionId,
                        ctx => ctx.MousePosition
                    ))
                    .Selector("Choose")
                        .Subtree(HuntMouseBehaviour(patrolPositionId, userMousePositionId))
                        .Subtree(PatrolBehaviour(patrolPositionId, userMousePositionId))
                    .End()
                .End()
                .Build();
        }

        public static IBehaviour<behaviours.BehaviourTreeBlackboardContext> HuntMouseBehaviour(string patrolPositionId, string mousePositionId)
        {
            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .Sequence("Hunt Target")
                    .Condition("Is mouse in reach?", BehaviourConditions.IsTargetPositionInReach(mousePositionId, 400))
                    .Do("Stop patrolling", BehaviourActions.RemoveTargetPosition(patrolPositionId))
                    .Do("Move to target", BehaviourActions.MoveToTargetPosition(mousePositionId))
                .End()
                .Build();
        }

        public static IBehaviour<behaviours.BehaviourTreeBlackboardContext> PatrolBehaviour(string patrolPositionId, string mousePositionId)
        {

            return FluentBuilder.Create<behaviours.BehaviourTreeBlackboardContext>()
                .Sequence("Patrol")
                    .Condition("Is mouse out of reach?", BehaviourConditions.IsTargetPositionOutOfReach(mousePositionId, 400))
                    .Do("Set random position", BehaviourActions.SetRandomNearbyTargetPosition(patrolPositionId, 400))
                    .Do("Patrol to position", BehaviourActions.MoveToTargetPosition(patrolPositionId))
                .End()
                .Build();
        }
    }


}
