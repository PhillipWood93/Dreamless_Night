using Godot;
using System;

public partial class Hud : CanvasLayer
{
	private TextureProgressBar _progressBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressBar = (TextureProgressBar)GetNode("HealthBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateHealthBar(double value)
	{
		_progressBar.Value = value;
	}
}
