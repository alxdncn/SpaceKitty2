using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadLightPixels : MonoBehaviour {

	WebCamShader webCamScript;

	[SerializeField] GameObject colliderPrefab;
	List<GameObject> colliderPool = new List<GameObject>();

	[SerializeField] float waitTimer = 3f;
	bool startedWebcam = false;

	float camSize;
	float camX;
	float camY;
	Vector2 camPos;

	void Awake(){
		webCamScript = GetComponent<WebCamShader> ();
		camX = Camera.main.pixelWidth;
		camY = Camera.main.pixelHeight;
		camPos = Camera.main.transform.position;
		camSize = Camera.main.orthographicSize;

		//I hate that 1.28 is a magic number I found by guessing and checking. What's the actual logic here?
		float colliderSize = (webCamScript.texReadIncrement/(camY * 1.28f)) * camSize;

		colliderPrefab.transform.localScale = new Vector3(colliderSize, colliderSize, colliderSize);
	}

	void Update(){
		if (waitTimer > 0) {
			waitTimer -= Time.deltaTime;
			return;
		} else if(!startedWebcam){
			startedWebcam = true;
			GameStateManager.instance.RunGame();
		}

		int numLightPixels = Mathf.Max(webCamScript.brightPixelPositions.Count, colliderPool.Count);

		Vector2 pos2D = new Vector2 ();
		Vector3 newPos = new Vector3 ();
		newPos.z = 0;

		for (int i = 0; i < numLightPixels; i++) {
			if (i >= webCamScript.brightPixelPositions.Count) {
				colliderPool [i].SetActive (false);
				continue;
			}

			GameObject obj;

			if (i >= colliderPool.Count) {
				obj = (GameObject)Instantiate (colliderPrefab);
				colliderPool.Add (obj);
			} else {
				obj = colliderPool [i];
				if (!obj.activeSelf)
					obj.SetActive (true);
			}

			pos2D = convertToWorldPoint (webCamScript.brightPixelPositions [i]);
			newPos.x = pos2D.x;
			newPos.y = pos2D.y;

			obj.transform.position = newPos;
		}
	}

	Vector2 convertToWorldPoint(Vector2 screenPos){
		float xFraction = (screenPos.x / camX) * 2f - 1f;;
		float yFraction = (screenPos.y / camY) * 2f - 1f;

		float x = camPos.x + (xFraction) * (camSize * camX / camY);
		float y = camPos.y + (yFraction) * (camSize);

		return new Vector2 (x, y);
	}
}
