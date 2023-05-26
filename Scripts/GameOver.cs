using Godot;
using System;

public partial class GameOver : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayAgain()
	{
		GetTree().ReloadCurrentScene();
	}


    public void OnQuit()
	{
		GetTree().Quit();
	}
}
