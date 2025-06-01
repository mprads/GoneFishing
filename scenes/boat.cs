using Godot;
using System;

public partial class boat : RigidBody3D { 
	[Export]
	public float float_force { get; set; } = 1.4f;
	[Export]
	public float water_drag { get; set; } = 0.05f;
	[Export]
	public float water_angular_drag { get; set; } = 0.1f;
	
	private Node3D probe_container;

	private const float GRAVITY = 9.8f;
	//Need to get water height from water mesh rather than hard coding
	private const float water_height = 14.0f;
	private bool submerged = false;

	public override void _Ready() {
		probe_container = GetNode<Node3D>("ProbeContainer");
	}

	public override void _PhysicsProcess(double delta) {
		submerged = false;
		foreach (Marker3D probe in probe_container.GetChildren()) {
			var depth = (water_height - probe.GlobalPosition.Y);
			if (depth > 0) {
				submerged = true;
				ApplyForce(Vector3.Up * float_force * GRAVITY * depth, probe.GlobalPosition - GlobalPosition);
			}
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state){
		if (submerged) {
			state.LinearVelocity *= 1 - water_drag;
			state.AngularVelocity *= 1 - water_angular_drag;
		}
	}
}
