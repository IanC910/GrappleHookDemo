using Godot;
using System;

public partial class LevelCamera : Camera2D {
    RigidBody2D player = null;

    [Export] int cameraHeightOffset = 400;

    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("../Player");
    }

    public override void _Process(double delta)
    {
        Rotation = player.Rotation;
        Position = new Vector2(player.Position.X, player.Position.Y - cameraHeightOffset);
    }
}
