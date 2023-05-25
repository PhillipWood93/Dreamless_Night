using Godot;
using System;
using System.Threading.Tasks;

public partial class Hud : CanvasLayer
{
	private TextureProgressBar _progressBar;
	private Transtioner _transtioner;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressBar = (TextureProgressBar)GetNode("HealthBar");
		_transtioner = (Transtioner)GetNode("Transtioner");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateHealthBar(double value)
	{
		_progressBar.Value = value;
	}

	public void PlayTranstionIn()
	{
		_transtioner?.PlayTranstionIn();
	}

	public async Task PlayTranstionOut()
	{
		await _transtioner?.PlayTranstionOut();
	}
}
