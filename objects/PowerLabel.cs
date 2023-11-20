using Godot;
using System;

public partial class PowerLabel : Label
{
	[Export]
	public ProjectileLauncher Launcher { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Launcher.PowerChanged += OnPowerChanged;
	}

	private void OnPowerChanged(float newPower) {
		Text = "POWER: " + newPower;
	}
}
