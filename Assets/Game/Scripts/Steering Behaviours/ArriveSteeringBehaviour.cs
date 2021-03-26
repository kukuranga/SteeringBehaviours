using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveSteeringBehaviour : SeekSteeringBehaviour
{
	public float SlowDownDistance = 1.0f;
	public float Deceleration = 2.5f;
	public float StoppingDistance = 0.1f;

	public override Vector3 calculateForce()
	{
		checkMouseInput();
		return CalculateArriveForce();
	}

	protected Vector3 CalculateArriveForce()
	{
		Vector3 toTarget = target - steeringAgent.transform.position;
		float distance = toTarget.magnitude;

		steeringAgent.reachedGoal = false;
		if (distance > SlowDownDistance)
		{
			return CalculateSeekForce();
		}
		else if (distance > StoppingDistance && distance <= SlowDownDistance)
		{
			toTarget.Normalize();

			float speed = distance / Deceleration;
			speed = (speed < steeringAgent.maxSpeed ? speed : steeringAgent.maxSpeed);

			speed = speed / distance;
			desiredVelocity = toTarget * speed;
			return desiredVelocity - steeringAgent.velocity;
		}

		steeringAgent.reachedGoal = true;
		return Vector3.zero;
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		DebugExtension.DebugCircle(target, Vector3.up, Color.red, SlowDownDistance);
	}
}
