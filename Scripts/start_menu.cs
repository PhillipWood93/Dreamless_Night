using Godot;
using System;

public partial class start_menu : Control
{
	[Export] private PackedScene _levelOne;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStartPressed()
	{
		GetTree().ChangeSceneToPacked(_levelOne);
	}
}
