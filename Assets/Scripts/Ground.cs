using Godot;
using System;

public partial class Ground : Area2D
{
	[Signal]
	public delegate void PlayerHitGroundEventHandler();

	void OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.PlayerHitGround);
	}
}
