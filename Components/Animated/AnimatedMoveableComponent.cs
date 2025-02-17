using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Godot;

namespace Components
{
    public enum Direction
    {
        Up = 0,
        LeftUp,
        RightUp,
        LeftDown,
        Down,
        RightDown,
        Left,
        Right
    }

    /**
     * A component that can be animated.
     *
     * Specifically it is a state machine that looks for child nodes which describe
     * what animations to run in that state and how (and what) that state should transition to.
     */
    public partial class AnimatedMoveableComponent : Component
    {
        public AnimatedMoveableComponent()
        {
            this.ComponentName = "AnimatedMoveableComponent";
        }

        [Export]
        public AnimatedMoveableState initialState = null;

        [Export]
        public AnimatedSprite2D Sprite = null;

        public FiniteStateMachine machine = null;

        public DirectionManager Direction = new DirectionManager();

        public override void _Ready()
        {

            var children = this.GetChildren().Where(node => node is AnimatedMoveableState).ToList();
            this.machine = new FiniteStateMachine(children);
            this.machine.Transition(this.initialState.name);
            this.Direction.SetDirection(Components.Direction.Up);
            base._Ready();
        }
        public override void _Process(double delta)
        {
            this.machine.Update(delta);
        }

    }

    public class DirectionManager
    {

        public Direction[] DirectionHistory = new Direction[2];


        public Direction GetCurrentDirection()
        {
            return this.DirectionHistory[0];
        }

        public Direction GetPreviousDirection()
        {
            return this.DirectionHistory[1];
        }

        public void SetDirection(Direction direction)
        {
            var current = this.GetCurrentDirection();
            if (current == direction)
            {
                return;
            }

            this.DirectionHistory[1] = current;
            this.DirectionHistory[0] = direction;
        }
        public void SetDirectionFromVector2(Vector2 direction)
        {
            var newDirection = this.GetDirectionFromVector2(direction);

            this.SetDirection(newDirection);
        }

        public Direction GetDirectionFromVector2(Vector2 direction)
        {

            if (direction.X < 0 && direction.Y == 0)
            {
                return Direction.Left;
            }
            else if (direction.X < 0 && direction.Y > 0)
            {
                return Direction.LeftDown;
            }
            else if (direction.X < 0 && direction.Y < 0)
            {
                return Direction.LeftUp;
            }
            else if (direction.X > 0 && direction.Y == 0)
            {
                return Direction.Right;
            }
            else if (direction.X == 0 && direction.Y < 0)
            {
                return Direction.Up;
            }
            else if (direction.X > 0 && direction.Y < 0)
            {
                return Direction.RightUp;
            }
            else if (direction.X == 0 && direction.Y > 0)
            {
                return Direction.Down;
            }
            else if (direction.X > 0 && direction.Y > 0)
            {
                return Direction.RightDown;
            }

            return Direction.Left;
        }

        public string GetCurrentDirectionName()
        {
            var directionName = Enum.GetName(typeof(Direction), this.GetCurrentDirection());
            return directionName;
        }

        public string GetPreviousDirectionName()
        {
            var previousDirectionName = Enum.GetName(typeof(Direction), this.GetPreviousDirection());
            return previousDirectionName;
        }

    }
}
