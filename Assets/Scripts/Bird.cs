using Godot;
using System;
using System.Numerics;

public partial class Bird : CharacterBody2D
{
	const int GRAVITY = 1000;
	const int MAX_VELOCITY = 600;
	const int FLAP_SPEED = -500;
	public bool flying = false;
	public bool falling = false;
	public Godot.Vector2 START_POS = new Godot.Vector2(100, 400);
	// Called when the node enters the scene tree for the first time.
	public void Reset()
	{
		falling = false;
		flying = false;
		Position = START_POS;
		Rotation = 0;	
	}
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        if (flying || falling)
		{
			float targetVelocity = Velocity.Y;
			targetVelocity += GRAVITY * (float)delta;
			Velocity = Velocity with {Y = targetVelocity};
			if (Velocity.Y> MAX_VELOCITY)
			{
				Velocity = Velocity with {Y = MAX_VELOCITY};
			}
			if (flying)
			{
				Rotation = Mathf.DegToRad(Velocity.Y * 0.05f);
			}
			else if (falling)
			{
				Rotation = Mathf.DegToRad(Mathf.Pi /2);
			}
			MoveAndCollide(Velocity * (float)delta);
		}
    }

	public void Flap()
	{
		Velocity = Velocity with { Y = FLAP_SPEED};
	}
}
