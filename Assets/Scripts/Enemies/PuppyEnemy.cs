using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppyEnemy : EnemyBaseClass {

	Vector3 startDirection;

	[SerializeField] float moveOutwardsTime = 10f;
	float moveOutwardsTimer = 0;

	GameObject motherShip;

	protected override void Awake(){
		base.Awake();
		SetColliderToMainObject ();
	}

	public override void Reset(){
		base.Reset();
		transform.position = motherShip.transform.position;
	}

	public void InitializeWithVector(Vector2 direction, GameObject mother){
		moveOutwardsTimer = 0f;
		startDirection = direction;
		motherShip = mother;
	}

	protected override void Move(){
		float t = Mathf.Clamp01(moveOutwardsTimer / moveOutwardsTime);
		Vector3 towardsKitty = (-GetVectorToKitty(transform).normalized * t)  + startDirection.normalized * (1 - t) * 2;
		transform.position += towardsKitty * movementSpeed * Time.deltaTime;

		if(moveOutwardsTimer < moveOutwardsTime){
			moveOutwardsTimer += Time.deltaTime;
		}
	}

	protected override IEnumerator WaitForDeathAnimationToEnd(float time){
		yield return new WaitForSeconds(time);
		motherShip.GetComponent<MotherShip>().PupIsDestroyed(this);
	}
}
