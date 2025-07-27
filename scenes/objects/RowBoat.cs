using Godot;
using System;

public partial class RowBoat : RigidBody3D { 
	[Export]
	public float FloatForce { get; set; } = 1.4f;
	[Export]
	public float WaterDrag { get; set; } = 0.05f;
	[Export]
	public float WaterAngularDrag { get; set; } = 0.1f;
	
	private Node3D ProbeContainer;

	private const float GRAVITY = 9.8f;
	//Need to get water height from water mesh rather than hard coding
	private const float WaterHeight = 1.5f;
	private bool Submerged = false;

	public override void _Ready() {
		ProbeContainer = GetNode<Node3D>("ProbeContainer");
	}

	public override void _PhysicsProcess(double delta) {
		Submerged = false;
		foreach (Marker3D probe in ProbeContainer.GetChildren()) {
			var depth = (WaterHeight - probe.GlobalPosition.Y);
			if (depth > 0) {
				Submerged = true;
				ApplyForce(Vector3.Up * FloatForce * GRAVITY * depth, probe.GlobalPosition - GlobalPosition);
			}
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state){
		if (Submerged) {
			state.LinearVelocity *= 1 - WaterDrag;
			state.AngularVelocity *= 1 - WaterAngularDrag;
		}
	}
}
