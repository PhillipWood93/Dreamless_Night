using Godot;
using System;

public partial class Boar : EnemyBase
{
    bool _isRunning = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        if (!_isRunning) return;
        base._PhysicsProcess(delta);
    }

    public override void OnScreenExited()
    {
        base.OnScreenExited();
    }

    public override void OnScreenEnter()
    {
        base.OnScreenEnter();
        _isRunning = true;
        _stateMachine.Travel("run");
    }

    public override void OnAttack(Node2D other)
    {
        base.OnAttack(other);
    }

    public override void StartAttack(Node2D body)
    {
        if (body.Name == "Player")
        {
            //_isRunning = false;
            // _stateMachine.Travel("idle");
            Health h = (Health)body.GetNode("Health");
            //body.ProcessMode = ProcessModeEnum.Disabled;
            h.SetHealth(-h.health);
        }
    }

    public override void StopAttack(Node2D body)
    {
        
    }
}
