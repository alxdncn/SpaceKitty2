using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urinate : MonoBehaviour {

	public List<GameObject> pees = new List<GameObject>();
	public int maxPee;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pees.Add (Instantiate ((Resources.Load ("P-lazer")) as GameObject, gameObject.transform.position, Quaternion.identity));
		pees[pees.Count-1].GetComponent<Rigidbody2D>().AddForce(Vector3.left * 500);

	}
}
