using Godot;
using System;

public partial class Pipe : Area2D
{
	[Signal] 
	public delegate void PlayerHitPipeEventHandler();

	[Signal] 
	public delegate void PlayerScoredEventHandler();

	public void OnScoreAreaEntered(Node2D body)
	{
		EmitSignal(SignalName.PlayerScored);
	}

	public void OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.PlayerHitPipe);
	}
}
