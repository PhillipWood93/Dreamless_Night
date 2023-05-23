using Godot;
using System;

public partial class portal : Area2D
{
	[Export]
	public PackedScene SceneToTranstion;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BodyEntered += this.OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnBodyEntered(Node2D body)
	{
		GetTree().ChangeSceneToPacked(SceneToTranstion);
	}
}
