using UnityEngine;
using System.Collections;

public class scrPerson : MonoBehaviour
{
	public Terrain Landscape;

	public Transform Body, Head;
	protected Vector3 look;
	protected Vector2 direction;

	public float Health { get; private set; }
	public float Hunger;
	public float Thirst;
	public float MinSanics, MaxSanics;
	public float Sanics { get; private set; }

	protected virtual void Awake()
	{
		Health = 100.0f;
	}

	protected virtual void Update()
	{
		Sanics = Mathf.Lerp (MinSanics, MaxSanics, Health / 100.0f);

		// Reduce health based on hunger.
		Hunger += Time.deltaTime * (rigidbody.velocity.magnitude / Sanics + 1);
		if (Hunger <= 0)
			Health += Time.deltaTime * Mathf.Max (-Hunger, 1);
		else
			Health -= Time.deltaTime * Hunger;

		// Reduce health based on thirst.
		Thirst += Time.deltaTime * (rigidbody.velocity.magnitude / Sanics + 1);
		if (Thirst <= 0)
			Health += Time.deltaTime * Mathf.Max (-Thirst, 1);
		else
			Health -= Time.deltaTime * Thirst;

		// Turn the body towards the look yaw.
		Body.eulerAngles = new Vector3(0, Mathf.LerpAngle(Body.eulerAngles.y, look.y, Time.deltaTime * 5), 0);
		
		// Turn the head to the look direction quicker than the body.
		Head.rotation = Quaternion.Lerp (Head.rotation, Quaternion.Euler (look), Time.deltaTime * 10);
	}

	protected virtual void FixedUpdate()
	{
		RaycastHit hit;
		Physics.Raycast (transform.position + Vector3.up * 10, Vector3.down, out hit, 100, 1 << LayerMask.NameToLayer ("Landscape"));
		float adjustedSanics = Sanics * (1 + Vector3.Dot(direction, hit.normal) * 0.5f);
		rigidbody.AddForce (Body.TransformDirection(new Vector3(direction.x, 0, direction.y)) * adjustedSanics * Time.fixedDeltaTime);
		rigidbody.position = new Vector3(rigidbody.position.x, Landscape.SampleHeight(rigidbody.position) + transform.localScale.y, rigidbody.position.z);
	}

	protected void Eat(float amount)
	{
		Hunger -= amount;
	}

	protected void Drink(float amount)
	{
		Thirst -= amount;
	}
}
