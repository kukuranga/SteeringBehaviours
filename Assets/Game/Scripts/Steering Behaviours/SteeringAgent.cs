using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class SteeringAgent : MonoBehaviour
{
	public enum SummingMethod
	{
		WeightedAverage,
		Prioritized,
	};
	public SummingMethod summingMethod = SummingMethod.WeightedAverage;

	public bool UseGravity = true;
	public bool UseRootMotion = true;

	public float mass = 1.0f;
	public float maxSpeed = 1.0f;
	public float maxForce = 10.0f;

	public Vector3 velocity = Vector3.zero;

	private List<SteeringBehaviourBase> steeringBehaviours = new List<SteeringBehaviourBase>();

	public float angularDampeningTime = 5.0f;
	public float deadZone = 10.0f;

	[HideInInspector] public bool reachedGoal = false;

	private Animator animator;
	private CharacterController characterController;

	private void Start()
	{
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();

		steeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>());
		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			behaviour.Init(this);
		}
	}

	private void OnAnimatorMove()
	{
		if (Time.deltaTime != 0.0f && UseRootMotion)
		{
			Vector3 animationVelocity = animator.deltaPosition / Time.deltaTime;
			characterController.Move((transform.forward * animationVelocity.magnitude) * Time.deltaTime);
			if (UseGravity)
			{
				characterController.Move(Physics.gravity * Time.deltaTime);
			}
		}
	}

	private void Update()
	{
		Vector3 steeringForce = calculateSteeringForce();
		steeringForce.y = 0.0f;

		if (reachedGoal == true)
		{
			velocity = Vector3.zero;
			animator.SetFloat("Speed", 0.0f);
		}
		else
		{
			Vector3 acceleration = steeringForce / mass;
			velocity = velocity + (acceleration * Time.deltaTime);
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

			float speed = velocity.magnitude;
			animator.SetFloat("Speed", speed);

			if (!UseRootMotion)
			{
				characterController.Move(velocity * Time.deltaTime);
				if (UseGravity)
				{
					characterController.Move(Physics.gravity * Time.deltaTime);
				}
			}
		}

		if (velocity.magnitude > 0.0f)
		{
			float angle = Vector3.Angle(transform.forward, velocity);
			if (Mathf.Abs(angle) <= deadZone)
			{
				transform.LookAt(transform.position + velocity);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(velocity),
													  Time.deltaTime * angularDampeningTime);
			}
		}
	}

	private Vector3 calculateSteeringForce()
	{
		Vector3 totalForce = Vector3.zero;

		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			if (behaviour.enabled)
			{
				switch(summingMethod)
				{
					case SummingMethod.WeightedAverage:
						totalForce = totalForce + (behaviour.calculateForce() * behaviour.weight);
						totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
						break;

					case SummingMethod.Prioritized:
						break;
				}

			}
		}

		return totalForce;
	}
}
