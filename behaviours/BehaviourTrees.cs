using System.Xml.Linq;
using BehaviourTree;
using BehaviourTree.FluentBuilder;
using components;
using Godot;
using systems;

namespace behaviours
{

    public static class BehaviourTrees
    {

        public static IBehaviour<BehaviourTreeBlackboardContext> FollowsMouseBehaviour()
        {
            var userMousePositionId = UserMouseSystem.MouseEntityName;
            var patrolPositionId = "Patrol";

            return FluentBuilder.Create<BehaviourTreeBlackboardContext>()
                .Sequence("Follow the mouse")
                    .Do("Target the user mouse", BehaviourActions.SetEntityAsTargetFactory(userMousePositionId))
                    .Selector("Choose")
                        .Subtree(HuntMouseBehaviour(patrolPositionId, userMousePositionId))
                        .Subtree(PatrolBehaviour(patrolPositionId, userMousePositionId))
                    .End()
                .End()
                .Build();
        }

        public static IBehaviour<BehaviourTreeBlackboardContext> HuntMouseBehaviour(string patrolPositionId, string mousePositionId)
        {
            return FluentBuilder.Create<BehaviourTreeBlackboardContext>()
                .Sequence("Hunt Target")
                    .Condition("Is mouse in reach?", BehaviourConditions.IsTargetEntityInReach(mousePositionId, 400))
                    .Do("Stop patrolling", BehaviourActions.RemoveTargetPosition(patrolPositionId))
                    .Do("Move to target", (ctx) => BehaviourActions.MoveToTargetEntityPosition(ctx, mousePositionId))
                .End()
                .Build();
        }

        public static IBehaviour<BehaviourTreeBlackboardContext> PatrolBehaviour(string patrolPositionId, string mousePositionId)
        {

            return FluentBuilder.Create<BehaviourTreeBlackboardContext>()
                .Sequence("Patrol")
                    .Selector("Patrol")
                        .Sequence("Set new waypoint?")
                            .Condition("Has reached random target or has no waypoint?", (ctx) => {
                                var missingTarget = !ctx.EntityManager.HasComponentWithId<TargetPositionComponent>(ctx.Entity, patrolPositionId);
                                var targetReached = BehaviourConditions.IsTargetPositionInReach(patrolPositionId, 40)(ctx);
                                var outcome = missingTarget || targetReached;
                                GD.Print($"Has reached random target or has no waypoint? {outcome} [missingTarget: {missingTarget} || targetReached: {targetReached}] ");
                                return outcome;
                            })
                            .Do("Set new random waypoint", BehaviourActions.SetRandomNearbyTargetPosition(patrolPositionId, 40, 400))
                        .End()
                        .Sequence("Move to waypoint")
                            .Condition("Is mouse out of reach?", BehaviourConditions.IsTargetEntityOutOfReach(mousePositionId, 400))
                            .Do("Move towards waypoint", BehaviourActions.MoveToTargetPositionFactory(patrolPositionId))
                        .End()
                    .End()
                .End()
                .Build();
        }
    }


}
