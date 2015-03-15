using UnityEngine;
using System.Collections;

public class scrFollower : scrPerson
{
	protected override void Update ()
	{
		base.Update ();

		look = Quaternion.LookRotation (scrPlayer.Instance.transform.position - transform.position, Vector3.up).eulerAngles;
	}

	protected override void FixedUpdate()
	{
		RaycastHit hit;
		Physics.Raycast (transform.position + Vector3.up * 10, Vector3.down, out hit, 100, 1 << LayerMask.NameToLayer ("Landscape"));
		float adjustedSanics = Sanics * (1 + Vector3.Dot(direction, hit.normal) * 0.5f);
		rigidbody.AddForce (transform.TransformDirection(new Vector3(direction.x, 0, direction.y)) * adjustedSanics * Time.fixedDeltaTime);
		rigidbody.position = new Vector3(rigidbody.position.x, Landscape.SampleHeight(rigidbody.position) + transform.localScale.y, rigidbody.position.z);
	}
	
	public void MoveTowards(Vector3 position)
	{
		direction = (new Vector2(position.x, position.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
	}

	public void Stop()
	{
		direction = Vector2.zero;
	}

}
