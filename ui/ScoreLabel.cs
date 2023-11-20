using Godot;
using System;

public partial class ScoreLabel : Label
{
	[Export]
	public Score Score { get; private set;}

    public override void _Ready()
    {
        Score.ScoreChanged += OnScoreChanged;
		Text = "SCORE: " + Score.Value;
    }

	public void OnScoreChanged(int newScore) {
		Text = "SCORE: " + newScore;
	}
}
