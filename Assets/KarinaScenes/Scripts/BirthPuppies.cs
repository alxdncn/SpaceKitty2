using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPuppies : MonoBehaviour {

	public static List<GameObject> puppies = new List<GameObject> ();
	//static GameObject aPup;

	public int pupCount; 

	// Use this for initialization
	void Start () {
		//aPup = Resources.Load ("Puppy") as GameObject;
		GiveBirth ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GiveBirth(){
		for (int i = 0; i <= pupCount; i++) {
			puppies.Add (Instantiate ((Resources.Load ("Puppy")) as GameObject, gameObject.transform.position, Quaternion.identity));
		}
	
	}

}
