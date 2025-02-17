using System;
using Actors;
using Godot;

public partial class YellowPeonIdleState : YellowPeonBaseState
{

	public override void Enter()
	{
		base.Enter();
		var direction = this.Component.Direction.GetCurrentDirectionName();
		this.Sprite.Play($"idle_{direction.ToLower()}");
	}

	public override void Update(double delta)
	{
		base.Update(delta);

		if (this.machine.CurrentState.name != "idle")
		{
			return;
		}

		if (this.IsMoving())
		{
			this.machine.Transition("run");
		}
	}

	public override void Exit()
	{
		base.Exit();
		GD.Print("Yellow Peon Idle State Exited");
	}
}
