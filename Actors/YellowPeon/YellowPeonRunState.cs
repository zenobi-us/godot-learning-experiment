using System;
using Components;
using Godot;

namespace Actors
{
	public partial class YellowPeonRunState : YellowPeonBaseState
	{
		public new string AnimationName = "run";

		public override void Enter()
		{
			base.Enter();
			var direction = this.Component.Direction.GetCurrentDirectionName();
			this.Sprite.Play($"run_${direction.ToLower()}");
		}

		public override void Update(double delta)
		{
			base.Update(delta);

			if (this.machine.CurrentState.name != "run")
			{
				return;
			}

			if (!this.IsMoving())
			{
				this.machine.Transition("idle");
				return;
			}

			var direction = this.Component.Direction.GetCurrentDirectionName().ToLower();
			GD.Print($"Yellow Peon is moving in direction: {direction}");
			GD.Print($"Current Animation: {this.Sprite.Animation}");
			GD.Print($"Desired Animation: run_{direction}");

			if (this.Sprite.Animation != $"run_{direction}")
			{
				this.Sprite.Play($"run_{direction}");
			}

			this.Component.Direction.SetDirectionFromVector2(this.Moveable.Velocity);

		}

		public override void Exit()
		{
			base.Exit();
			GD.Print("Yellow Peon Run State Exited");
		}
	}
}
