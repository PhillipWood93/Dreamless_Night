using Godot;
using System;

public partial class player : CharacterBody2D
{
	[ExportSubgroup("Movement")]
	[Export] 
	public float Speed = 300.0f;
	
	[Export] 
	public float JumpVelocity = -400.0f;

	[ExportSubgroup("Combat")]
	[Export]
	public bool _isAttacking = false;

	[Export]
	public float damage { get; private set; } = 100.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private bool _isDead = false;

	private AnimationTree _animTree;
	private AnimationNodeStateMachinePlayback _stateMachine;
	private Area2D _attackZone;
	private Health _health;
	private Hud _hud;
	private Sprite2D _sprite;

    public override void _Ready()
    {
        base._Ready();

        _sprite = (Sprite2D)GetNode("Sprite2D");

        _health = (Health)GetNode("Health");
		_health.OnHealthChanged += OnHealthChanged;


		_attackZone = (Area2D)GetNode("AttackZone");
		_attackZone.BodyEntered += OnAttack;

		_animTree = (AnimationTree) GetNode("AnimationTree");
		_animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");

		_hud = (Hud)GetNode("Hud");
		_hud.UpdateHealthBar(_health.health);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Input.IsActionPressed("attack") && IsOnFloor())
		{
			_stateMachine.Travel("attack");
			_isAttacking=true;
			AttackOver();
		}
    }

	private async void AttackOver()
	{
		await ToSignal(_animTree, AnimationTree.SignalName.AnimationFinished);
		_isAttacking = false;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if (_isAttacking || _isDead) return;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{

			velocity.Y = JumpVelocity;
		}

		if(velocity.Y < 0)
		{
            _animTree.Set("parameters/conditions/jump_over", false);
            _animTree.Set("parameters/conditions/jump_land", false);
            _animTree.Set("parameters/conditions/is_jumping", true);
		}
		else if(velocity.Y > 0)
		{
			_animTree.Set("parameters/conditions/is_falling", true);
		}
		else if(velocity.Y == 0)
		{
			_animTree.Set("parameters/conditions/jump_over", true);
            _animTree.Set("parameters/conditions/is_falling", false);
            _animTree.Set("parameters/conditions/is_jumping", false);
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
		
		_animTree.Set("parameters/run/blend_position", direction.X);
        _animTree.Set("parameters/jump_start/blend_position", direction.X);
        _animTree.Set("parameters/jump_air/blend_position", direction.X);
        _animTree.Set("parameters/jump_fall/blend_position", direction.X);
        _animTree.Set("parameters/jump_end/blend_position", direction.X);
        if (direction.X != 0)
		{
			velocity.X = direction.X * Speed;
			_attackZone.Scale = new Vector2(direction.X, 1);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}


		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnAttack(Node2D body)
	{
		if (body.IsInGroup("enemies"))
		{
			Health h = (Health)body.GetNode("Health");
			h.SetHealth(h.health - damage);
			GD.Print(body.Name + " Has " + h.health);
		}
	}

	private void OnHealthChanged()
	{
		if(_health.health <= 0)
		{
			Die();
		}
		_hud.UpdateHealthBar(_health.health);
	}

	private async void Die()
	{
		_isDead = true;
		_animTree.Set("parameters/conditions/isdead", _isDead);
		await ToSignal(_animTree, AnimationTree.SignalName.AnimationFinished);
		ProcessMode = ProcessModeEnum.Disabled;
		//TODO: Show GameOver Screen...
	}
}
