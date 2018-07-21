using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : EnemyBaseClass {

	float timer = 0f;

	Vector3 crossVector;

	[SerializeField] float sineMagnitude = 0.1f;

	[SerializeField] GameObject puppyPrefab;
	[SerializeField] float puppySpawnTime = 1f;
	float puppySpawnTimer = 0f;

	[SerializeField] float waitToSpawnPupsTime = 4f;
	float waitToSpawnPupsTimer = 0f;

	List<PuppyEnemy> activePups = new List<PuppyEnemy> ();
	List<PuppyEnemy> inactivePups = new List<PuppyEnemy> ();

	[SerializeField] GameObject sheild;

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
		waitToSpawnPupsTimer = 0f;
	}

	protected override void Move ()
	{
		base.Move ();
		transform.position += crossVector * Mathf.Cos (timer) * Time.deltaTime * sineMagnitude;
		timer += Time.deltaTime;
		SpawnPuppy();
	}

	void SpawnPuppy(){
		if(waitToSpawnPupsTimer < waitToSpawnPupsTime){
			waitToSpawnPupsTimer += Time.deltaTime;
			return;
		}

		if(puppySpawnTimer >= puppySpawnTime){
			puppySpawnTimer = 0f;
			RemoveShield ();
			if(inactivePups.Count > 0){
				PuppyEnemy newPup = inactivePups[0];
				inactivePups.RemoveAt(0);
				activePups.Add(newPup);
				InitializePup(newPup);
			} else{
				GameObject newPup = (GameObject)Instantiate(puppyPrefab, transform.position, Quaternion.identity);
				PuppyEnemy pupScript = newPup.GetComponent<PuppyEnemy>();
				InitializePup(pupScript);
			}
			
		}

		puppySpawnTimer += Time.deltaTime;
	}

	void RemoveShield(){
		sheild.SetActive (false);
	}


	protected virtual IEnumerator ReturnShield (){
		yield return new WaitForSeconds(1f);
		sheild.SetActive (true);
	}

	void InitializePup(PuppyEnemy pup){
		int randInt = Random.Range(0,2);
		Vector3 direction = crossVector;
		if(randInt == 0){
			direction = -direction;
		}
		StartCoroutine (ReturnShield());
		pup.InitializeWithVector(direction, this.gameObject);
		pup.Reset();
	}

	public void PupIsDestroyed(PuppyEnemy pup){
		activePups.Remove (pup);
		inactivePups.Add (pup);
		pup.gameObject.SetActive (false);
	}

	public override void DestroyEnemy(){
		DestroyAllPups();
		base.DestroyEnemy();
	}

	void DestroyAllPups(){
		for(int i = activePups.Count - 1; i >= 0; i--){
			activePups[i].DestroyEnemy();
		}
	}
}
