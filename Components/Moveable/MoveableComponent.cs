using System;
using System.Diagnostics;
using Core;
using Godot;

public partial class MoveableComponent : Component
{
	public MoveableComponent()
	{
		this.ComponentName = "MoveableComponent";
	}

	public Vector2 velocity = new Vector2(0, 0);
	// public override void Update(float delta)
	// {
	// 	position += velocity * delta;
	// }

	[Signal]
	public delegate void OnLeftGroundEventHandler();

	[Signal]
	public delegate void OnGroundedEventHandler();

	[Signal]
	public delegate void OnDirectionChangedEventHandler(Vector2 direction);

	/**
	* The velocity of the moveable object, which determines its speed and direction.
	*/
	public double MAX_SPEED = 120;
	/**
	* The minimum speed at which the moveable object can move.
	* This helps prevent the object from moving too slowly, especially
	* when it's stationary or near the ground.
	*/
	public double MIN_SPEED = 15;
	/**
	* The multiplier applied to the speed when moving in air.
	* This is useful for creating a more realistic feel of movement
	* in games where gravity plays a significant role.
	*/
	public double AIR_MULTIPLIER = 0.7f;

	/**
	* The acceleration applied to the moveable object when moving in air.
	* This controls how quickly the object accelerates as it moves upward.
	*/
	public double AIR_ACCELERATION = 150;
	/**
	* The acceleration applied to the moveable object when moving downward.
	* This controls how quickly the object decelerates as it moves downward.
	*/
	public double FALL_GRAVITY = 150;
	/**
	* The acceleration applied to the moveable object when moving
	* upward, which is used for jumping. This value should be set high enough
	* to allow the player to jump without being too heavy or difficult to control.
	*/
	public double RISE_GRAVITY = 900.0f;

	/**
	* The acceleration applied to the moveable object when moving
	* horizontally. This controls how quickly the object accelerates as it moves left or right.
	*/
	public double ACCELERATION = 900;

	[Export]
	public Vector2 input { get; set; }


	CharacterBody2D moveable;


	public override void _Ready()
	{
		base._Ready();

		if (this.GetParent().GetClass() != "CharacterBody2D")
		{
			Debug.WriteLine("CharacterBody2D is not the parent of this component.");
			throw new Exception("CharacterBody2D is not the parent of this component.");
		}

		moveable = this.GetParent() as CharacterBody2D;

	}

	public override void _Process(double delta)
	{
		this.Move(delta);
	}

	public void Accelerate(double delta, Vector2 input)
	{
		float speed = (float)this.ACCELERATION * (float)delta;
		double x = Mathf.MoveToward(
				this.moveable.Velocity.X,
				this.MAX_SPEED * input.X,
				speed
			);
		double y = Mathf.MoveToward(
			this.moveable.Velocity.Y,
			this.MAX_SPEED * input.Y,
			speed
		);

		this.moveable.Velocity = new Vector2((float)x, (float)y);
	}

	public void Move(double delta)
	{
		this.Accelerate(delta, this.input);
		this.moveable.MoveAndSlide();
	}

}

