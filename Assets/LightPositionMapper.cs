#define TEST

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//The color that we're checking for
public enum PossibleColors{
	Red,
	Green,
	Blue,
	Cyan,
	Magenta,
	Yellow
}

public class LightPositionMapper : MonoBehaviour {
	//Get the webcam render texture
	//Cycle through it to find the center position of the light source
	//Get position of light on x, y of screen
	//Convert that to being world space x, y in the hypercube range
	//Use overall saturation to determine z-depth

	private static LightPositionMapper instance;
	public static LightPositionMapper GetInstance(){
		return instance;
	}

	WebCamTexture webCamTex;

	//I'll want to make this customizable at start
	float saturationThreshold = 0.4f;
	const float SATURATION_INCREMENT = 0.05f;

	public PossibleColors colorToTrack;

	Color trackingColor;
	Vector3 adjustedColor;

	const int PIXEL_INCREMENT = 2;
	int cameraWidthBuffer;
	int cameraHeightBuffer;
	const float BUFFER_PERCENTAGE = 0.1f;

	Transform brushTransform;

	const float MIN_X = -9f;
	const float MAX_X = 9f;
	const float MIN_Y = -4f;
	const float MAX_Y = 5f;
	const float MIN_Z = -6f;
	const float MAX_Z = 6f;

	const float LERP_SPEED = 4f;

	int numberPixelsAboveThreshold;
	const int MIN_RUN_SATURATION = 30;
	int minSaturation;
	int maxSaturation;
	bool setMin = false;
	bool setMax = false;

	//test textures
	[SerializeField] Renderer testTexture;
	[SerializeField] Renderer outputTexture;

	[SerializeField] Text instructionsText;
	[SerializeField] GameObject arrowInstructionsText;

	// Use this for initialization
	void Awake () {
		instance = this;

		webCamTex = new WebCamTexture ();
		webCamTex.Play();

		cameraWidthBuffer = Mathf.RoundToInt (webCamTex.width * BUFFER_PERCENTAGE);
		cameraHeightBuffer = Mathf.RoundToInt (webCamTex.height * BUFFER_PERCENTAGE);

		brushTransform = GameObject.Find ("Brush").GetComponent<Transform> ();

		#if !TEST
		testTexture.gameObject.SetActive(false);
		#else
		testTexture.material.SetFloat("_Buffer", BUFFER_PERCENTAGE);
		testTexture.material.SetFloat("_SaturationThreshold", saturationThreshold);
//		outputTexture.material.SetFloat("_SaturationThreshold", saturationThreshold);
		#endif

		colorToTrack = (PossibleColors)PlayerPrefs.GetInt ("Tracking Color", 0);
		SetTrackingColor ();

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 position = ReadWebCamData();

		if (numberPixelsAboveThreshold > MIN_RUN_SATURATION/PIXEL_INCREMENT) {
			brushTransform.localPosition = Vector3.Lerp(brushTransform.localPosition, position, LERP_SPEED * Time.deltaTime);
			brushTransform.LookAt(position, -brushTransform.parent.forward);
		}

		AdjustSaturationThreshold ();
		SetZDepthRange ();
	}

	void SetTrackingColor(){
		switch (colorToTrack) {
		case PossibleColors.Red:
			trackingColor = Color.red;
			break;
		case PossibleColors.Green:
			trackingColor = Color.green;
			break;
		case PossibleColors.Blue:
			trackingColor = Color.blue;
			break;
		case PossibleColors.Cyan:
			trackingColor = Color.cyan;
			break;
		case PossibleColors.Yellow:
			trackingColor = new Color(1, 1, 0);
			break;
		case PossibleColors.Magenta:
			trackingColor = Color.magenta;
			break;
		}

		adjustedColor = new Vector3 (trackingColor.r, trackingColor.g, trackingColor.b);

		bool cMY = adjustedColor.sqrMagnitude > 1 ? true : false;

		for (int i = 0; i < 3; i++) {
			if (adjustedColor [i] <= 0)
				adjustedColor [i] = -1;
			if (cMY && adjustedColor [i] == 1)
				adjustedColor [i] = 0.5f;
		}

		Debug.Log (adjustedColor);

		testTexture.material.SetVector ("_AdjustColor", adjustedColor);

	}

	Vector3 ReadWebCamData(){
		numberPixelsAboveThreshold = 0;
		int xTotal = 0;
		int yTotal = 0;

		for (int i = 0; i < webCamTex.width; i += PIXEL_INCREMENT) {
			for (int j = 0; j < webCamTex.height; j += PIXEL_INCREMENT) {
				Color webCamCol = webCamTex.GetPixel(i, j);
				float lightness = (webCamCol.r * adjustedColor.x) 
					+ (webCamCol.g * adjustedColor.y) 
					+ (webCamCol.b * adjustedColor.z);
				if (lightness > saturationThreshold) {
					numberPixelsAboveThreshold++;
					xTotal += i;
					yTotal += j;
				} 
			}
		}

		#if TEST
		testTexture.material.SetTexture ("_WebCamTexture", webCamTex);

//		outputTexture.material.SetTexture ("_WebCamTexture", webCamTex);
		#endif

		if (numberPixelsAboveThreshold > 0 && setMax) {
			int averageX = Mathf.RoundToInt (xTotal / numberPixelsAboveThreshold);
			int averageY = Mathf.RoundToInt (yTotal / numberPixelsAboveThreshold);

			return GetPositionFromWebCamData( new Vector2 (averageX, averageY));
		}
			
		return Vector3.zero;

	}

	Vector3 GetPositionFromWebCamData(Vector2 webCamCoords){

		float xPos = Mathf.Clamp01((webCamCoords.x - cameraWidthBuffer) / (webCamTex.width - cameraWidthBuffer));
		float yPos = Mathf.Clamp01((webCamCoords.y - cameraHeightBuffer) / (webCamTex.height - cameraHeightBuffer));

		float xRange = MAX_X - MIN_X;
		float yRange = MAX_Y - MIN_Y;

		xPos = (xPos * xRange) + MIN_X;
		yPos = (yPos * yRange) + MIN_Y;

		float zPos = GetDepthPosition ();

		return new Vector3 (-xPos, yPos, zPos);
	}

	void AdjustSaturationThreshold(){
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			saturationThreshold = Mathf.Clamp01(saturationThreshold + SATURATION_INCREMENT);
			testTexture.material.SetFloat("_SaturationThreshold", saturationThreshold);
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			saturationThreshold = Mathf.Clamp01(saturationThreshold - SATURATION_INCREMENT);
			testTexture.material.SetFloat("_SaturationThreshold", saturationThreshold);
		}
	}

	float GetDepthPosition(){
		float zRange = MAX_Z - MIN_Z;

		float zPos = Mathf.Clamp01((float)numberPixelsAboveThreshold / ((float)maxSaturation - (float)minSaturation));
	
		zPos = (zPos * zRange) + MIN_Z;

		return zPos;
	}

	void SetZDepthRange(){
		if (setMin && !setMax && Input.GetKeyDown (KeyCode.Space)) {
			maxSaturation = numberPixelsAboveThreshold;	
			setMax = true;
			instructionsText.text = "Hold Space to paint texture. WASD keys move viewpoint. Q & E change paint color.";
			StartCoroutine (SetInstructionsTextInactive ());
		}
		if (!setMin && Input.GetKeyDown (KeyCode.Space)) {
			minSaturation = numberPixelsAboveThreshold;
			setMin = true;
			arrowInstructionsText.SetActive (false);
			instructionsText.text = "Hold tracking object close to camera and press space to set FAR plane value.";
		}
		if (setMin && setMax && Input.GetKeyUp (KeyCode.Space)) {
			ProceduralMesh.GetInstance().canPaint = true;
		}
	}

	IEnumerator SetInstructionsTextInactive(){
		yield return new WaitForSeconds (5f);
		instructionsText.gameObject.SetActive (false);
	}

}
