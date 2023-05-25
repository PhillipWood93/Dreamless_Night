using Godot;
using System;

public partial class CreditLabel : Label
{
	[Export] private float _speed = 20.0f;

	private float _screenWidth = ProjectSettings.GetSetting("display/window/size/viewport_width").AsSingle();
	private float _screenHeight = ProjectSettings.GetSetting("display/window/size/viewport_height").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		double y = Position.Y - _speed * delta;
        Position = new Vector2((_screenWidth * .5f) - (Size.X * .5f), (float)y);
	}
}
