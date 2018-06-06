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
    
    [SerializeField] float startSpawnTime = 2f;
    [SerializeField] float endSpawnTime = 0.5f;
    float currentSpawnTime;
    float spawnTimer = 0;
    
    [SerializeField] float startBurstTime = 20f;
    [SerializeField] float endBurstTime = 5f;
    float currentBurstTime;
    float burstTimer = 0;
    
    [SerializeField] int startBurstNumber = 4;
    [SerializeField] int endBurstNumber = 20;
    int burstNumber;
        
    public float maxY;
    public float minY;
    public float maxX;
    public float minX;

	void Awake(){
        currentSpawnTime = startSpawnTime;
        currentBurstTime = startBurstTime;
        burstNumber = startBurstNumber;
		GameStateManager.instance.beatGame += DestroyAllEnemies;
	}

	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        burstTimer += Time.deltaTime;
                
        float roundFraction = GameStateManager.instance.RoundProgress;
        
        
        if(spawnTimer > currentSpawnTime){
            SpawnEnemy();
            spawnTimer = 0;
            currentSpawnTime = (startSpawnTime * (1 - roundFraction) + endSpawnTime * roundFraction);
        }
        if(burstTimer > currentBurstTime){
            for(int i = 0; i < burstNumber; i++){
                SpawnEnemy();
            }
            burstTimer = 0;
            currentBurstTime = (startBurstTime * (1 - roundFraction) + endBurstTime * roundFraction);
            burstNumber = Mathf.RoundToInt(startBurstNumber * (1 - roundFraction) + endBurstNumber * roundFraction);
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
		for (int i = 0; i < activeEnemies.Count; i++) {
			activeEnemies [i].DestroyEnemy ();
		}
	}

	//This is a little circuitous, but this will be called at the end of the enemy instance destroy function
	public void EnemyIsDestroyed(EnemyBaseClass enemy){
		activeEnemies.Remove (enemy);
		inactiveEnemies.Add (enemy);
		enemy.gameObject.SetActive (false);
	}
}
