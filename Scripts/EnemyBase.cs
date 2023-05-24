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
	public float attackDuration { get; private set; } = 2.0f;

	[Export]
	public float damage { get; private set; } = 10.0f;

	[ExportCategory("SFX/VFX")]
	[Export]
	private AudioStream _deathSound;

	private bool _isAttacking = false;
	
	private bool _isDead = false;
	private Health _health;

	private Area2D _detectionZone;
	private Area2D _attackZone;

	private AnimationTree _animTree;
	private AnimationNodeStateMachinePlayback _stateMachine;

	private AudioStreamPlayer _audioPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = (Health)GetNode("Health");
		_health.OnHealthChanged += OnHealthChanged;

		_detectionZone = (Area2D)GetNode("DetectZone");
		_detectionZone.BodyEntered += StartAttack;
		_detectionZone.BodyExited += StopAttack;

		_attackZone = (Area2D)GetNode("AttackZone");
		_attackZone.BodyEntered += OnAttack;

		_animTree = (AnimationTree)GetNode("AnimationTree");
		_animTree.Active = true;
		_stateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");

		_audioPlayer = (AudioStreamPlayer)GetNode("Audio");
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
		if(body.Name == "Player" && !_isDead)
		{
			_isAttacking = true;
			_animTree.Set("parameters/conditions/isattacking", _isAttacking);
		}
	}

	private async void StopAttack(Node2D body)
	{
		if(body.Name == "Player")
		{
			await ToSignal(GetTree().CreateTimer(attackDuration), Timer.SignalName.Timeout);
			_isAttacking = false;
			_animTree.Set("parameters/conditions/isattacking", false);
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
		_audioPlayer.Stream = _deathSound;
		_animTree.Set("parameters/conditions/isdead", _isDead);
		_audioPlayer.Play();
		await ToSignal(_animTree, AnimationTree.SignalName.AnimationFinished);
		this.QueueFree();
	}

    private void OnScreenExited()
	{
		this.QueueFree();
	}
}
