using Godot;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public partial class portal : Area2D
{
	[ExportSubgroup("Scene Transition")]
	[Export]
	public PackedScene SceneToTranstion;

	[ExportSubgroup("Spawn Settings")]
	[Export] private bool _isSpawner = true;
	[Export(PropertyHint.Range, "5, 10")] private float _minSpawnRate = 5.0f;
	[Export(PropertyHint.Range, "10, 15")] private float _maxSpawnRate = 10.0f;
	//This will take a random index from the array and spawn that creature...
	[Export] private bool _randomSpawn = true;
	[Export] private PackedScene[] _enemiesToSpawn;

	private Timer _spawnTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		SetProcess(false);
		_spawnTimer = (Timer)GetNode("SpawnTimer");
		this.BodyEntered += this.OnBodyEntered;
	}

    private void OnSpawnTimerTimeout()
	{
		if (_randomSpawn)
		{
			SpawnRandomEnemy();
		}
		else
		{
			SpawnInSequance();
		}
	}

    public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			GetTree().ChangeSceneToPacked(SceneToTranstion);
		}
	}

	private void SpawnRandomEnemy()
	{
		int i = GD.RandRange(0, _enemiesToSpawn.Length - 1);
		Spawn(i);
		CalculateRandomSpawnTime();
	}

	private void CalculateRandomSpawnTime()
	{
		_spawnTimer.WaitTime = GD.RandRange(_minSpawnRate, _maxSpawnRate);
	}

	/// <summary>
	/// This function spawns the enemies in order...
	/// </summary>
	private async void SpawnInSequance()
	{
		for (int i = 0; i < _enemiesToSpawn.Length; ++i)
		{
			Spawn(i);
			CalculateRandomSpawnTime();
			await ToSignal(_spawnTimer, Timer.SignalName.Timeout);

		}
	}

	private void Spawn(int i)
	{
        Node2D c = (Node2D)_enemiesToSpawn[i].Instantiate();
        c.Position = Position;
        GetTree().Root.AddChild(c);
    }
}
