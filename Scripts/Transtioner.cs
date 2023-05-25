using Godot;
using System;
using System.Threading.Tasks;

public partial class Transtioner : Sprite2D
{

	private AnimationPlayer _animPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_animPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void PlayTranstionIn()
	{
		this.Show();
        GetTree().Paused = true;
        _animPlayer?.Play("transtion_in");
		await ToSignal(_animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        GetTree().Paused = false;
        this.Hide();
	}

	public async Task PlayTranstionOut()
	{
		this.Show();
		GetTree().Paused = true;
        _animPlayer?.Play("transtion_out");
        await ToSignal(_animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		GetTree().Paused = false;
		this.Hide();
    }
}
