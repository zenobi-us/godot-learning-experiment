using System;
using components;
using Godot;

namespace behaviours
{

    public static class BehaviourConditions
    {
        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionInReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                TargetPositionComponent target = ctx.EntityManager.GetComponent<TargetPositionComponent>(ctx.Entity, positionId);
                PositionComponent position = ctx.EntityManager.GetComponent<PositionComponent>(ctx.Entity);
                if (position == null || target == null)
                {
                    GD.Print("IsTargetPositionInReach: No position or target");
                    return false;
                }

                var distance = position.Position.DistanceTo(target.Position);
                var isInReach = distance < distanceThreshold;
                GD.Print($"IsTargetPositionInReach: {isInReach}");
                return isInReach;
            }
            ;
        }
        public static Func<BehaviourTreeBlackboardContext, bool> IsTargetPositionOutOfReach(string positionId, int distanceThreshold)
        {
            return (ctx) =>
            {
                TargetPositionComponent target = ctx.EntityManager.GetComponent<TargetPositionComponent>(ctx.Entity, positionId);
                PositionComponent position = ctx.EntityManager.GetComponent<PositionComponent>(ctx.Entity);
                if (position == null || target == null)
                {
                    GD.Print("IsTargetPositionOutOfReach: No position or target");
                    return true;
                }

                var distance = position.Position.DistanceTo(target.Position);
                var isOutOfReach = distance > distanceThreshold;
                GD.Print($"IsTargetPositionOutOfReach: {isOutOfReach}");
                return isOutOfReach;
            };
        }

    }

}
