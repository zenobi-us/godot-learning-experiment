using BehaviourTree;
using BehaviourTree.FluentBuilder;
using components;
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
                    .Do("Target the user mouse", BehaviourActions.SetEntityAsTargetFactory(UserMouseSystem.MouseEntityName))
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
                    .Do("Move to target", BehaviourActions.MoveToTargetPositionFactory(mousePositionId))
                .End()
                .Build();
        }

        public static IBehaviour<BehaviourTreeBlackboardContext> PatrolBehaviour(string patrolPositionId, string mousePositionId)
        {

            return FluentBuilder.Create<BehaviourTreeBlackboardContext>()
                .Sequence("Patrol")
                    .Condition("Is mouse out of reach?", BehaviourConditions.IsTargetEntityOutOfReach(mousePositionId, 400))
                    .Do("Set random position", BehaviourActions.SetRandomNearbyTargetPosition(patrolPositionId, 400))
                    .Do("Patrol to position", BehaviourActions.MoveToTargetPositionFactory(patrolPositionId))
                .End()
                .Build();
        }
    }


}
