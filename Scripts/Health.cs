using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Health : Node
{
	[Signal]
	public delegate void OnHealthChangedEventHandler();

	[Export]
	public float maxHealth { get; private set; } = 100.0f;

	public float health { get; private set; }

    public override void _Ready()
    {
        base._Ready();
		health = maxHealth;
    }

    public float Gethealth() { return health; }

	public void SetHealth(float value)
	{
		health = Math.Clamp(value, 0, maxHealth);
		EmitSignal(SignalName.OnHealthChanged);
	}
	
	
}
