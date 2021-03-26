using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetPursuitSteeringBehaviour : ArriveSteeringBehaviour
{
	public SteeringAgent PursuitObject;
	public Vector3 Offset;

	public override Vector3 calculateForce()
	{
		if (PursuitObject != null)
		{
			Vector3 worldSpaceOffset = PursuitObject.transform.rotation * Offset.normalized +
									   PursuitObject.transform.position;

			Vector3 offsetPosition = worldSpaceOffset - steeringAgent.transform.position;
			float lookAheadTime = offsetPosition.magnitude / (steeringAgent.maxSpeed + PursuitObject.velocity.magnitude);

			target = worldSpaceOffset + PursuitObject.velocity * lookAheadTime;

			return base.calculateForce();
		}

		return Vector3.zero;
	}
}
