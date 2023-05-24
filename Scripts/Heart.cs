using Godot;
using System;

public partial class Heart : Area2D
{
    [Export]
    public float amountToHeal { get; private set; } = 50.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.BodyEntered += OnBodyEnter;
    }

    public async void OnBodyEnter(Node2D other)
    {
        if (other.Name == "Player")
        {
            AnimationPlayer anim = (AnimationPlayer)GetNode("AnimationPlayer");
            anim.Play("pickedup");
            Health h = (Health)other.GetNode("Health");
            h.SetHealth(h.health + amountToHeal);
            await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
            this.QueueFree();
        }
    }
}
