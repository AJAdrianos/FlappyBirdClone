using Godot;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

public partial class Game : Node
{
	[Export] PackedScene pipeScene;
	Bird bird;
	Ground ground;
	Timer timer;
	Label scoreLabel;
	bool gameRunning = false;
	bool gameOver = false;
	int scroll;
	int score;
	const int SCROLL_SPEED = 2;
	const int PIPE_DELAY  = 100;
	const int PIPE_RANGE  = 200;
	List<Area2D> pipes = new List<Area2D>();
	Godot.Vector2I screenSize;
	int groundHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ground = GetNode<Ground>("Ground"); 
		ground.PlayerHitGround += OnGroundHit;
		bird = GetNode<Bird>("Bird");
		timer = GetNode<Timer>("Timer");
		scoreLabel = GetNode<Label>("Score");
		screenSize = GetWindow().Size;
		groundHeight = ((Sprite2D)GetNode("Ground/Sprite2D")).Texture.GetHeight();
		NewGame();
	}

	public void GeneratePipes()
	{
		var gameObject = pipeScene.Instantiate();
		var pipe = gameObject as Area2D;
		pipe.Position = pipe.Position with { X = screenSize.X + PIPE_DELAY};
		pipe.Position = pipe.Position with {Y = (screenSize.Y - groundHeight) / 2  + GD.RandRange(-PIPE_RANGE, PIPE_RANGE)};
		Pipe pipeScript = pipe as Pipe;
		pipeScript.PlayerHitPipe += OnPipeHit;
		pipeScript.PlayerScored += OnScored;
		AddChild(pipe);
		pipes.Add(pipe);
	}

    private void OnScored()
    {
        score += 1;
		scoreLabel.Text = "SCORE: " + score.ToString();
    }

    public void OnPipeHit()
	{
		bird.falling = true;
		StopGame();
	}

	public void OnTimerTimeout()
	{
		GeneratePipes();
	}

	public void OnGroundHit()
	{
		bird.falling = false;
		StopGame();
	}

	public void StopGame()
	{
		bird.flying = false;
		gameRunning = false;
		gameOver = true;
		timer.Stop();
	}

	public void NewGame()
	{
		gameRunning = false;
		gameOver=false;
		scroll = 0;
		score = 0;
		scoreLabel.Text = "SCORE: " + score.ToString();
		GetTree().CallGroup("pipes", "queue_free");
		pipes.Clear();
		GeneratePipes();
		bird.Reset();
	}

    public override void _Process(double delta)
    {
		if(gameRunning)
		{
			scroll += SCROLL_SPEED;
			if (scroll >= screenSize.X)
			{
				scroll = 0;
			}
			ground.Position = ground.Position with { X = -scroll};
			// GD.Print(pipes.Count);
			foreach (var pipe in pipes)
			{
				pipe.Position = pipe.Position with { X = (float) (pipe.Position.X - scroll * delta) };
			}
		}
    }

	public void CheckTop()
	{
		if(bird.Position.Y < 0)
		{
			bird.falling = true;
			StopGame();
		}
	}

    public void StartGame()
	{
		gameRunning = true;
		bird.flying = true;
		bird.Flap();
		timer.Start(); 
	}
    public override void _Input(InputEvent @event)
    {
		if(gameOver == false)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			{
				if(mouseEvent.ButtonIndex == MouseButton.Left)
				{
					if (gameRunning == false)
					{
						StartGame();
					}
					else 
					{
						if(bird.flying)
						{
							bird.Flap();
							CheckTop();
						}
					}
				}
			}
		}
    }
}
