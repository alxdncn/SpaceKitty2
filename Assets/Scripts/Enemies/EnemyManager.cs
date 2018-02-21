using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	static EnemyManager instance;
	public static EnemyManager Instance{
		get{
			if (instance == null) {
				instance = FindObjectOfType<EnemyManager>();
			}
			return instance;
		}
		private set { instance = value; }
	}

	[SerializeField] EnemyBaseClass[] enemyPrefabBag;
	List<EnemyBaseClass> currentEnemyBag = new List<EnemyBaseClass>();

	List<EnemyBaseClass> allEnemies = new List<EnemyBaseClass>();
	List<EnemyBaseClass> activeEnemies = new List<EnemyBaseClass> ();
	List<EnemyBaseClass> inactiveEnemies = new List<EnemyBaseClass> ();

//	System.Type[] enemySubclassList = ReflectionHelper.GetSubclasses (typeof(EnemyBaseClass));

	void Awake(){
//		for (int i = 0; i < 20; i++) {
//			SpawnEnemy ();
//		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			SpawnEnemy ();
		}
	}

	void ShuffleAndReplenishBag(){
		currentEnemyBag.Clear ();

		for (int i = 0; i < enemyPrefabBag.Length; i++) {
			currentEnemyBag.Add (enemyPrefabBag [i]);
		}

		for (int i = 0; i < currentEnemyBag.Count; i++) {
			EnemyBaseClass temp = currentEnemyBag [i];
			int randomIndex = Random.Range (i, currentEnemyBag.Count);
			currentEnemyBag [i] = currentEnemyBag [randomIndex];
			currentEnemyBag [randomIndex] = temp;
		}
	}

	void SpawnEnemy(){
		if (currentEnemyBag.Count == 0) {
			ShuffleAndReplenishBag ();
		}

		EnemyBaseClass newEnemy = currentEnemyBag[0];
		currentEnemyBag.RemoveAt (0);

		//If there is already an inactive enemy of the same type, active it and jump out
		for (int i = 0; i < inactiveEnemies.Count; i++) {
			if (inactiveEnemies [i].GetType () == newEnemy.GetType()) {
				newEnemy = inactiveEnemies [i];
				activeEnemies.Add (newEnemy);
				inactiveEnemies.RemoveAt (i);
				newEnemy.Reset (); 
				return;
			}
		}

		//If we make it through the for loop, that means we need to make a new enemy of that type
		GameObject newEnemyObj = (GameObject)Instantiate(newEnemy.gameObject);

		//This is a bit weird, but we want to change what this is pointing to from the prefab to the new instance
		newEnemy = newEnemyObj.GetComponent<EnemyBaseClass>();

		allEnemies.Add (newEnemy);
		activeEnemies.Add (newEnemy);
//		newEnemy.Reset (); //probably don't want this, should just use Awake
	}

	//Call the destroy function on all enemies, which will trigger animation and then call EnemyIsDestroyed on this class
	public void DestroyAllEnemies(){
		for (int i = 0; i < allEnemies.Count; i++) {
			allEnemies [i].DestroyEnemy ();
		}
	}

	//This is a little circuitous, but this will be called at the end of the enemy instance destroy function
	public void EnemyIsDestroyed(EnemyBaseClass enemy){
		activeEnemies.Remove (enemy);
		inactiveEnemies.Add (enemy);
		enemy.gameObject.SetActive (false);
	}
}
