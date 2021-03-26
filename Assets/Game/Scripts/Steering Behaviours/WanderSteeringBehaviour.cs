using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteeringBehaviour : SteeringBehaviourBase
{
	public float WanderDistance = 2.0f;
	public float WanderRadius = 1.0f;
	public float WanderJitter = 20.0f;
	private Vector3 WanderTarget;

	private void Start()
	{
		float theta = (float)Random.value * Mathf.PI * 2;
		WanderTarget = new Vector3(WanderRadius * Mathf.Cos(theta),
								   0.0f,
								   WanderRadius * Mathf.Sin(theta));
	}

	public override Vector3 calculateForce()
	{
		float jitterThisTimeSlice = WanderJitter * Time.deltaTime;

		WanderTarget = WanderTarget + new Vector3(Random.Range(-1.0f, 1.0f) * jitterThisTimeSlice,
												  0.0f,
												  Random.Range(-1.0f, 1.0f) * jitterThisTimeSlice);
		WanderTarget.Normalize();
		WanderTarget *= WanderRadius;

		target = WanderTarget + new Vector3(0, 0, WanderDistance);
		target = steeringAgent.transform.rotation * target + steeringAgent.transform.position;

		Vector3 wanderForce = (target - steeringAgent.transform.position).normalized;
		return wanderForce *= steeringAgent.maxSpeed;
	}

	private void OnDrawGizmos()
	{
		Vector3 circleCenter = transform.rotation * new Vector3(0, 0, WanderDistance) + transform.position;

		DebugExtension.DrawCircle(circleCenter, Vector3.up, Color.red, WanderRadius);
		Debug.DrawLine(transform.position, circleCenter, Color.yellow);
		Debug.DrawLine(transform.position, target, Color.blue);
	}
}
