using Godot;
using System;

public partial class Player : CharacterBody3D {
	[Export]
	public float HorizontalSensitivity = 0.5f;
	[Export]
	public float VerticalSensitivity = 0.5f;
	[Export]
	public float Speed = 6.0f;
	public const float JumpVelocity = 4.5f;

	private Node3D Visuals;
	private Node3D CameraMount;

	public override void _Ready() {
		Visuals = GetNode<Node3D>("Visuals");
		CameraMount = GetNode<Node3D>("CameraMount");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseMotion) {
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * HorizontalSensitivity));
			Visuals.RotateY(Mathf.DegToRad(mouseMotion.Relative.X * HorizontalSensitivity));
			CameraMount.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * VerticalSensitivity));
		}
	}

	public override void _PhysicsProcess(double delta) {
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			Visuals.LookAt(Position + direction);
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
