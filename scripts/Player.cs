using Godot;
using System;
using System.Runtime.ConstrainedExecution;

public partial class Player : RigidBody2D
{
    bool onGround = true;

    // All distance units are px (1 m = 100 px)

    const float RUN_ACC = 600;
    const float MIDAIR_RUN_ACC_FACTOR = 0.5f;
    const float BRAKE_ACC = 1000;

    const float MAX_RUN_SPEED = 600;
    const float JUMP_ACC = 10000;

    public override void _Ready()
    {
        Area2D playerFeetArea = GetNode<Area2D>("PlayerFeetArea");
        playerFeetArea.BodyEntered += OnPlayerFeetAreaEntered;
        playerFeetArea.BodyExited += OnPlayerFeetAreaExited;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // Movement Control
        int moveDirection = 0;
        if (Input.IsKeyPressed(Key.A))
        {
            moveDirection -= 1;
        }
        if (Input.IsKeyPressed(Key.D))
        {
            moveDirection += 1;
        }

        // Running
        if (moveDirection == Math.Sign(LinearVelocity.X) || LinearVelocity.X == 0)
        {
            if (Math.Abs(LinearVelocity.X) < MAX_RUN_SPEED)
            {
                float lateralAcceleration = moveDirection * RUN_ACC * (onGround ? 1 : MIDAIR_RUN_ACC_FACTOR);
                ApplyCentralForce(new Vector2(Mass * lateralAcceleration, 0));
            }
        }
        // Braking
        else
        {
            ApplyCentralForce(new Vector2(Mass * -Math.Sign(LinearVelocity.X) * BRAKE_ACC, 0));
        }

        // Jumping
        if (onGround && Input.IsKeyPressed(Key.Space))
        {
            ApplyCentralForce(new Vector2(0, -Mass * JUMP_ACC));
        }
    }

    private void OnPlayerFeetAreaEntered(Node2D body)
    {
        GD.Print("Land");
        onGround = true;
    }

    private void OnPlayerFeetAreaExited(Node2D body)
    {
        GD.Print("Leave ground");
        onGround = false;
    }
}
