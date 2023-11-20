using Godot;
using System;

public partial class ProjectileLauncher : Node2D
{
	[Signal]
	public delegate void PowerChangedEventHandler(float BasePower);

	[Export]
	public Node2D Aiming { get; private set; }

	[Export]
	public PackedScene ProjectileScene { get; private set; }

	[Export]
	public StringName ShootAction { get; private set; } = "Shoot";

	[Export]
	public StringName ProjectilesParentGroup { get; private set; } = "ProjectilesParent";

	[Export]
	public float BasePower { get; set; } = 30000f;

	[Export]
	public Sprite2D Sprite { get; private set; } 

	/// <summary>
	/// Multiply this by the Launch Power to get the bonus power
	/// that is added to the projectile at time of launch
	/// </summary>
	[Export]
	public float TimePowerMultiplier { get; set; } = 1.25f;

	public float LaunchPower { 
		get => _launchPower;
		set {
			_launchPower = value;
			EmitSignal("PowerChanged", _launchPower);
		}
	}

	public bool IsCharging {
		get => _isCharging;
		set {
			_isCharging = value;

			if(_isCharging == false) ChargeTime = 0.0f;
		}
	}

	public float ChargeTime {
		get => _chargeTime;
		set {
			_chargeTime = value;

			Sprite.SelfModulate = new Color(
				_startingSpriteColor.R,
				_startingSpriteColor.G - (_chargeTime / 5),
				_startingSpriteColor.B - (_chargeTime / 5),
				_startingSpriteColor.A
			);

			LaunchPower = BasePower + (BasePower * _chargeTime * TimePowerMultiplier);
		}
	}

	private Node _projectilesParent;
	private Vector2 _aimDirection;
	private bool _isCharging = false;
	private float _chargeTime = 0.0f;
	private float _launchPower;
	private Color _startingSpriteColor; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetDeferred("LaunchPower", BasePower);
		_projectilesParent = GetTree().GetFirstNodeInGroup(ProjectilesParentGroup);
		_startingSpriteColor = Sprite.SelfModulate;

		if(_projectilesParent == null) GD.PushWarning("No projectiles parent found in projectiles group " + ProjectilesParentGroup);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		_aimDirection = GlobalPosition.DirectionTo(mousePosition);
		Aiming.Rotation = GetAngleTo(mousePosition);

		if(_isCharging){
			ChargeTime += (float) delta;
		}
	}

    public override void _Input(InputEvent @event)
    {
		if(@event.IsActionPressed(ShootAction)) {
			ChargeProjectile();
		} else if(@event.IsActionReleased(ShootAction) && _isCharging) {
			ShootProjectile(ProjectileScene);
		}
    }

	public void ChargeProjectile() {
		IsCharging = true;
	}

	public void ShootProjectile(PackedScene projectileToShoot) {
		Projectile projectileInstance = projectileToShoot.Instantiate<Projectile>();
		_projectilesParent.AddChild(projectileInstance);
		projectileInstance.GlobalPosition = GlobalPosition;
		Vector2 launchVector = _aimDirection * LaunchPower;
		projectileInstance.ApplyForce(launchVector);
		IsCharging = false;
	}
}
