using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : EnemyBaseClass {

	float timer = 0f;

	Vector3 crossVector;

	[SerializeField] float sineMagnitude = 0.1f;

	protected override void Awake ()
	{
		base.Awake ();
		SetColliderToMainObject ();
		crossVector = Vector3.Cross (GetVectorToKitty (transform).normalized, Vector3.forward);
	}

	public override void Reset ()
	{
		base.Reset ();
		crossVector = Vector3.Cross (GetVectorToKitty (transform).normalized, Vector3.forward);
	}

	protected override void Move ()
	{
		base.Move ();
		transform.position += crossVector * Mathf.Cos (timer) * Time.deltaTime * sineMagnitude;
		timer += Time.deltaTime;
	}
}
