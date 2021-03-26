using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObstacleAvoidSteeringBehaviour : SteeringBehaviourBase
{
	[System.Serializable]
	public class Feeler
	{
		public float distance;
		public Vector3 offset;
	};
	public List<Feeler> feelers = new List<Feeler>();
	public LayerMask layerMask;

	public override Vector3 calculateForce()
	{
		RaycastHit hit;
		Ray ray;

		foreach(Feeler feeler in feelers)
		{
			Vector3 feelerPosition = transform.rotation * feeler.offset + transform.position;
			ray = new Ray(feelerPosition, transform.forward);
			if (Physics.Raycast(ray, out hit, feeler.distance, layerMask.value))
			{
				Vector3 otherPosition = TerrainManager.Instance.FindClosestTreePosition(hit.point);
				Vector3 collisionPoint = Vector3.Project(otherPosition - transform.position, transform.forward) + transform.position;
				float strength = 1.0f + ((collisionPoint.magnitude - feeler.distance) / feeler.distance);
				return (collisionPoint - otherPosition).normalized * strength;
			}
		}

		return Vector3.zero;
	}

	private void OnDrawGizmos()
	{
		foreach (Feeler feeler in feelers)
		{
			Vector3 feelerPosition = transform.rotation * feeler.offset + transform.position;
			Debug.DrawLine(feelerPosition, transform.forward * feeler.distance + feelerPosition, Color.blue);
		}
	}
}
