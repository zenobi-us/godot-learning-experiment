using System;
using components;
using Godot;

namespace behaviours
{

    public static class BehaviourConditions
    {

        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetEntityInReach(string targetEntityId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetEntityDistance(ctx, targetEntityId);
                var result = distance < distanceThreshold;

                return result;
            };
        }

        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetEntityOutOfReach(string targetEntityId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetEntityDistance(ctx, targetEntityId);
                var result = distance > distanceThreshold;
                GD.Print($"IsTargetEntityOutOfReach: {result}");
                return result;
            };
        }


        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionInReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetPositionDistance(ctx, positionId);
                var result = distance < distanceThreshold;
                return result;
            };
        }

        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionOutOfReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetPositionDistance(ctx, positionId);
                var result = distance > distanceThreshold;
                return result;
            };
        }

        private static float GetTargetEntityDistance(
            BehaviourTreeBlackboardContext context,
            string targetEntityId
        )
        {
            var target = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity, targetEntityId);
            if (target == null || target.TargetEntity == null)
            {
                return float.MaxValue;
            }

            var targetPositionComponent = context.EntityManager.GetComponent<PositionComponent>(target.TargetEntity);
            if (targetPositionComponent == null)
            {
                return float.MaxValue;
            }

            PositionComponent positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            var distance = positionComponent.Position.DistanceTo(targetPositionComponent.Position);

            return distance;
        }

        private static float GetTargetPositionDistance(
            BehaviourTreeBlackboardContext context,
            string positionId
        )
        {
            TargetPositionComponent target = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity, positionId);
            PositionComponent position = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            if (position == null || target == null)
            {
                return float.MaxValue;
            }

            var distance = position.Position.DistanceTo(target.Position);

            return distance;
        }
    }

}
