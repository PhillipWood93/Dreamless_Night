using Godot;
using System;

public partial class EnemyBase : CharacterBody2D
{
	[ExportCategory("Movement")]
	[Export]
	public float speed { get; private set; } = 150.0f;

	[Export]
	public int direction { get; private set; } = -1;

	[ExportCategory("Detection/Attacking")]
	[Export]
	public float attackRate { get; private set; } = .5f;

	[Export]
	public float damage { get; private set; } = 10.0f;

	private bool _isAttacking = false;
	
	private bool _isDead = false;
	private Health _health;

	private Timer _attackTimer;
	private Area2D _detectionZone;
	private Area2D _attackZone;

	private AnimationTree _animTree;
	private AnimationNodeStateMachinePlayback _stateMachine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = (Health)GetNode("Health");
		_health.OnHealthChanged += OnHealthChanged;

		_attackTimer = (Timer)GetNode("AttackTimer");
		_attackTimer.Timeout += OnAttackTimeout;
		_attackTimer.WaitTime = attackRate;

		_detectionZone = (Area2D)GetNode("DetectZone");
		_detectionZone.BodyEntered += StartAttack;
		_detectionZone.BodyExited += StopAttack;

		_attackZone = (Area2D)GetNode("AttackZone");
		_attackZone.BodyEntered += OnAttack;

		_animTree = (AnimationTree)GetNode("AnimationTree");
		_animTree.Active = true;
		_stateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");
	}

    public override void _PhysicsProcess(double delta)
    {
		if (_isDead) return;

        base._PhysicsProcess(delta);

		Vector2 velocity = Velocity;

		if (!_isAttacking)
		{
			velocity.X = speed * direction;
			Velocity = velocity;
			MoveAndSlide();
		}
    }

	private void StartAttack(Node2D body)
	{
		if(body.Name == "Player")
		{
			_isAttacking = true;
			_attackTimer.Start();
			_stateMachine.Travel("attack");
		}
	}

	private void StopAttack(Node2D body)
	{
		if (_isAttacking)
		{
			_isAttacking = false;
			_attackTimer.Stop();
			_stateMachine.Travel("idle");
        }
	}

	private void OnAttack(Node2D other)
	{
		if(other.Name == "Player")
		{
			Health h = (Health)other.GetNode("Health");
			h.SetHealth(h.health - damage);
		}
	}

	private void OnAttackTimeout()
	{
		//GD.Print("Attack");
	}

	private void OnHealthChanged()
	{
		if(_health.health <= 0 && !_isDead)
		{
			Die();
		}
	}

	private async void Die()
	{
		_isDead = true;
		_animTree.Set("parameters/conditions/isdead", _isDead);
		await ToSignal(_animTree, AnimationTree.SignalName.AnimationFinished);
		this.QueueFree();
	}

    private void OnScreenExited()
	{
		this.QueueFree();
	}
}
