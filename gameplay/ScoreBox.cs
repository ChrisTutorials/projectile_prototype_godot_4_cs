using Godot;
using System;

public partial class ScoreBox : Node2D
{
	[Export]
	public Area2D ScoreArea { get; private set; }

	[Export]
	public Score Score { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreArea.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body){
		if(body is Projectile) {
			Score.Value += 1;
			body.QueueFree();
		}
	}
}
