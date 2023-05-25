using Godot;
using System;
using System.Threading.Tasks;

public partial class Hud : CanvasLayer
{
	private TextureProgressBar _progressBar;
	private Transtioner _transtioner;
	private Control _pauseMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressBar = (TextureProgressBar)GetNode("HealthBar");
		_transtioner = (Transtioner)GetNode("Transtioner");
		_pauseMenu = (Control)GetNode("PauseMenu");

		PlayTranstionIn();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("pause"))
		{
			PauseGame();
		}
    }

    public void OnHealthChanged(float health)
	{
		UpdateHealthBar((double) health);
	}

    public void PauseGame()
	{
		GetTree().Paused = true;
		_pauseMenu.Visible = true;
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
