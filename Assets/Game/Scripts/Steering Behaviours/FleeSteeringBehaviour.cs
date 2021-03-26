using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeSteeringBehaviour : SteeringBehaviourBase
{
	public float FleeDistance = 1.0f;
	public Transform EnemyTarget;

	private float sqrFleeDistance;
	private Vector3 desiredVelocity;

	private bool showGizmoArrows = true;

	public override Vector3 calculateForce()
	{
		sqrFleeDistance = FleeDistance * FleeDistance;
		desiredVelocity = (steeringAgent.transform.position - EnemyTarget.position);

		float sqrDistance = desiredVelocity.sqrMagnitude;
		if (sqrDistance > sqrFleeDistance)
		{
			showGizmoArrows = false;
			return Vector3.zero;
		}

		showGizmoArrows = true;

		desiredVelocity.Normalize();
		desiredVelocity *= steeringAgent.maxSpeed;
		return desiredVelocity - steeringAgent.velocity;
	}

	private void OnDrawGizmos()
	{
		if (steeringAgent != null && showGizmoArrows)
		{
			DebugExtension.DebugArrow(transform.position, desiredVelocity, Color.red);
			DebugExtension.DebugArrow(transform.position, steeringAgent.velocity, Color.blue);
		}

		if (EnemyTarget != null)
		{
			DebugExtension.DebugWireSphere(EnemyTarget.position, Color.green, FleeDistance);
		}
	}
}
