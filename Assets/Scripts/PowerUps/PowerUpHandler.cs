using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour {

	#region UNIVERSAL
	static PowerUpHandler instance;
	public static PowerUpHandler Instance{
		get{
			if (instance == null) {
				instance = FindObjectOfType<PowerUpHandler>();
			}
			return instance;
		}
		private set { instance = value; }
	}

	delegate void CoolDowns();
	event CoolDowns coolDowns = null;

	[SerializeField] PowerUpBaseClass[] powerUpBag;
	List<PowerUpBaseClass> currentPUBag = new List<PowerUpBaseClass>();
	List<PowerUpBaseClass> allPU = new List<PowerUpBaseClass>();
	List<PowerUpBaseClass> activePU = new List<PowerUpBaseClass> ();
	List<PowerUpBaseClass> inactivePU = new List<PowerUpBaseClass> ();
	[SerializeField] float minSpawnTime = 20f;
	[SerializeField] float maxSpawnTime = 30f;
	float spawnTime;
	float spawnTimer;

	void Awake(){
		spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
	}

	void Update(){
		if (coolDowns != null) {
			coolDowns ();
		}
		spawnTimer += Time.deltaTime;
		if(spawnTimer > spawnTime){
			spawnTimer = 0;
			spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
			SpawnPowerUp();
		}
	}

	void ShuffleAndReplenishBag<T>(T[] bagPreset, List<T> bagToFill){
		bagToFill.Clear ();

		for (int i = 0; i < bagPreset.Length; i++) {
			bagToFill.Add (bagPreset [i]);
		}

		for (int i = 0; i < bagToFill.Count; i++) {
			T temp = bagToFill [i];
			int randomIndex = Random.Range (i, bagToFill.Count);
			bagToFill [i] = bagToFill [randomIndex];
			bagToFill [randomIndex] = temp;
		}
	}

	void SpawnPowerUp(){
		if (currentPUBag.Count == 0) {
			ShuffleAndReplenishBag (powerUpBag, currentPUBag);
		}

		PowerUpBaseClass newPU = currentPUBag[0];
		currentPUBag.RemoveAt (0);

		//If there is already an inactive enemy of the same type, active it and jump out
		for (int i = 0; i < inactivePU.Count; i++) {
			if (inactivePU [i].GetType () == newPU.GetType()) {
				newPU = inactivePU [i];
				activePU.Add (newPU);
				inactivePU.RemoveAt (i);
				newPU.Reset (); 
				return;
			}
		}

		//If we make it through the for loop, that means we need to make a new enemy of that type
		GameObject newPUObj = (GameObject)Instantiate(newPU.gameObject);

		//This is a bit weird, but we want to change what this is pointing to from the prefab to the new instance
		newPU = newPUObj.GetComponent<PowerUpBaseClass>();

		allPU.Add (newPU);
		activePU.Add (newPU);
//		newEnemy.Reset (); //probably don't want this, should just use Awake
	}

	public void GotPowerup(PowerUpBaseClass powerUp){
		System.Type powerUpType = powerUp.GetType ();

		if (powerUpType.Equals (typeof(BrightenPowerUp))) {
			InitBrighten ();
		} else if (powerUpType.Equals (typeof(DestroyAllPowerUp))) {
			DestroyAllEnemies();
		} else if(powerUpType.Equals(typeof(HealthBackPowerUp))){
			GetHealthBack();
		} else if(powerUpType.Equals(typeof(ShieldPowerUp))){
			ActivateShield();
		} else{
			Debug.LogWarning ("This powerup type hasn't been added to the handler! " + powerUpType);
		}
	}

	public void SetInactivePUObject (PowerUpBaseClass powerUp, GameObject obj){
		System.Type powerUpType = powerUp.GetType ();

		inactivePU.Add(powerUp);
		activePU.Remove(powerUp);
		obj.SetActive(false);

		//May want to keep these in order to change what happens to each type when it is deactivated
		if(powerUpType.Equals(typeof(BrightenPowerUp))){
			// obj.SetActive (false);
		} else if(powerUpType.Equals(typeof(DestroyAllPowerUp))){
			// obj.SetActive (false);
		} else if(powerUpType.Equals(typeof(HealthBackPowerUp))){
			// obj.SetActive (false);
		} else if(powerUpType.Equals(typeof(ShieldPowerUp))){
			// obj.SetActive(false);
		} else{
			Debug.LogWarning ("This powerup type hasn't been added to the handler! " + powerUpType);
		}
	}
	#endregion


	#region BRIGHTEN
	[SerializeField] float brightnessChange = 0.05f;
	[SerializeField] float brightnessCDTime = 2f;
	float brightnessCDTimer = 0f;
	bool brightened = false;

	void InitBrighten(){
		if (!brightened) {
			WebCamShader.Instance.IncreaseBrightness (brightnessChange);
			coolDowns += BrightenCooldown;
			brightened = true;
		}

		brightnessCDTimer = brightnessCDTime;
	}

	void BrightenCooldown(){
		brightnessCDTimer -= Time.deltaTime;

		if (brightnessCDTimer <= 0) {
			WebCamShader.Instance.DecreaseBrightness (brightnessChange);
			coolDowns -= BrightenCooldown;
			brightened = false;
		}
	}
	#endregion

	#region DESTROY ALL
	void DestroyAllEnemies(){
		if(EnemyManager.Instance != null)
			EnemyManager.Instance.DestroyAllEnemies ();
	}
	#endregion

	#region HEALTH
	void GetHealthBack(){
		if(Kitty.instance.lives < 9){
			Kitty.instance.GetLife();
		}
	}
	#endregion

	#region SHIELD
	[SerializeField] GameObject shield;
	[SerializeField] float shieldActiveTime = 5f;
	float shieldActiveTimer = 0f;
	bool shieldActive = false;

	void ActivateShield(){
		if(!shieldActive){
			shield.SetActive(true);
			shieldActive = true;
			coolDowns += ShieldCooldown;
		}
		shieldActiveTimer = 0f;
	}

	void ShieldCooldown(){
		shieldActiveTimer += Time.deltaTime;

		if (shieldActiveTimer >= shieldActiveTime) {
			shield.SetActive(false);
			coolDowns -= ShieldCooldown;
			shieldActive = false;
		}
	}
	#endregion
}
