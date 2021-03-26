using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFollowSteeringBehaviour : ArriveSteeringBehaviour
{
	public float WaypointDistance = 0.5f;
	public bool loop = false; // Won't use here, 

	private int currentWaypointIndex = 0;
	private NavMeshPath path;

	public override void Init(SteeringAgent agent)
	{
		base.Init(agent);
		path = new NavMeshPath();
		target = steeringAgent.transform.position;
	}

	public override Vector3 calculateForce()
	{
		checkMouseInput();

		if (newTarget == true)
		{
			currentWaypointIndex = 0;
			NavMesh.CalculatePath(steeringAgent.transform.position, target, NavMesh.AllAreas, path);
			if (path.corners.Length > 0)
			{
				target = path.corners[0];
			}
			else
			{
				target = steeringAgent.transform.position;
			}
		}

		if (currentWaypointIndex != path.corners.Length && (target - steeringAgent.transform.position).magnitude < WaypointDistance)
		{
			currentWaypointIndex++;
			if (currentWaypointIndex < path.corners.Length)
			{
				target = path.corners[currentWaypointIndex];
			}
		}
		return CalculateArriveForce();
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		if (path != null)
		{
			for (int i = 1; i < path.corners.Length; i++)
			{
				Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.black);
			}
		}
	}
}
