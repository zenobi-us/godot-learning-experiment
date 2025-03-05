using System;
using System.Numerics;
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
                var isInReach = distance < distanceThreshold;

                GD.Print($"IsTargetPositionInReach: {isInReach}");
                return isInReach;
            };
        }

        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetEntityOutOfReach(string targetEntityId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetEntityDistance(ctx, targetEntityId);
                var isInReach = distance < distanceThreshold;

                GD.Print($"IsTargetPositionInReach: {isInReach}");
                return isInReach;
            };
        }


        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionInReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetPositionDistance(ctx, positionId);
                var isInReach = distance < distanceThreshold;
                GD.Print($"IsTargetPositionInReach: {isInReach}");
                return isInReach;
            };
        }

        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionOutOfReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                var distance = GetTargetPositionDistance(ctx, positionId);
                var isOutOfReach = distance > distanceThreshold;
                GD.Print($"IsTargetPositionOutOfReach: {isOutOfReach}");
                return isOutOfReach;
            };
        }

        private static float GetTargetEntityDistance(
            BehaviourTreeBlackboardContext context,
            string targetEntityId
        )
        {
            TargetEntityComponent target = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity, targetEntityId);
            if (target == null)
            {
                return float.MaxValue;
            }

            if (target.TargetEntity == null)
            {
                return float.MaxValue;
            }

            TargetPositionComponent targetPositionComponent = context.EntityManager.GetComponent<TargetPositionComponent>(target.TargetEntity);
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
                GD.Print("IsTargetPositionInReach: No position or target");
                return float.MaxValue;
            }

            var distance = position.Position.DistanceTo(target.Position);

            return distance;
        }
    }

}
