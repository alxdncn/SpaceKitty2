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
	CoolDowns coolDowns = null;

//	System.Type[] powerUpSubclassList = ReflectionHelper.GetSubclasses (typeof(PowerUpBaseClass));

	void Update(){
		if (coolDowns != null) {
			coolDowns ();
		}
	}

	public void GotPowerup(PowerUpBaseClass powerUp){
		System.Type powerUpType = powerUp.GetType ();

		if (powerUpType.Equals (typeof(BrightenPowerUp))) {
			InitBrighten ();
		} else if (powerUpType.Equals (typeof(DestroyAllPowerUp))) {
			EnemyManager.Instance.DestroyAllEnemies ();
		} else {
			Debug.LogWarning ("This powerup type hasn't been added to the handler!");
		}
	}

	public void SetInactivePUObject (PowerUpBaseClass powerUp, GameObject obj){
		System.Type powerUpType = powerUp.GetType ();

		if(powerUpType.Equals(typeof(BrightenPowerUp))){
			obj.SetActive (false);
		} else if(powerUpType.Equals(typeof(DestroyAllPowerUp))){
			obj.SetActive (false);
		} else {
			Debug.LogWarning ("This powerup type hasn't been added to the handler!");
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

}
