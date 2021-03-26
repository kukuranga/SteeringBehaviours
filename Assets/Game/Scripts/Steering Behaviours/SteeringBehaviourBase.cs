using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
	public float weight = 1.0f;
	public Vector3 target = Vector3.zero;
	public bool useMouseInput = true;

	public abstract Vector3 calculateForce();

	[HideInInspector] protected SteeringAgent steeringAgent;

	protected bool newTarget;

	public virtual void Init(SteeringAgent agent)
	{
		steeringAgent = agent;
	}

	protected void checkMouseInput()
	{
		newTarget = false;
		if (Input.GetMouseButton(0) && useMouseInput)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				target = hit.point;
				newTarget = true;
			}
		}
	}

}
