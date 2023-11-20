using Godot;
using System;

public partial class Projectile : RigidBody2D
{
    [Export]
    public float TimeToLive { get; private set; } = 3f;

    private Timer _freeTimer;

    public override void _Ready()
    {
        _freeTimer = new Timer();
        AddChild(_freeTimer);
        _freeTimer.WaitTime = TimeToLive;
        _freeTimer.Timeout += OnTimeout;
        _freeTimer.Start();
    }

    public void OnTimeout(){
        QueueFree();
    }
}
