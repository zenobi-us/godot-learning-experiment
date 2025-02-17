using System;
using System.Buffers;
using Core;
using Godot;

namespace Components
{
    public partial class AnimatedMoveableState : FiniteState
    {
        public CharacterBody2D Moveable;

        public string AnimationName;

        public AnimatedSprite2D Sprite;

        public AnimatedMoveableComponent Component;

        public override void _Ready()
        {
            if (this.Owner.GetClass() != "CharacterBody2D")
            {
                throw new System.Exception("Owner must be a CharacterBody2D");
            }

            this.Component = this.GetParent<AnimatedMoveableComponent>();
            this.Moveable = (CharacterBody2D)this.Owner;
            this.Sprite = this.GetParent<AnimatedMoveableComponent>().Sprite;
            base._Ready();
        }

        public override void Enter()
        {
        }

        public override void Update(double delta)
        {
        }

        public override void Exit() { }

    }
}
