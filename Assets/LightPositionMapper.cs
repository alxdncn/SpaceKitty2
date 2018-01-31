#define TEST

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	Color trackingColor;
	Vector3 adjustedColor;

	const int PIXEL_INCREMENT = 2;
	int cameraWidthBuffer;
	int cameraHeightBuffer;
	const float BUFFER_PERCENTAGE = 0.1f;

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
	
		#if !TEST
		testTexture.gameObject.SetActive(false);
		#else
		testTexture.material.SetFloat("_Buffer", BUFFER_PERCENTAGE);
		testTexture.material.SetFloat("_SaturationThreshold", saturationThreshold);
		#endif

	}
	
	// Update is called once per frame
	void Update () {

		ReadWebCamData();

		AdjustSaturationThreshold ();
	}

	void ReadWebCamData(){
		numberPixelsAboveThreshold = 0;
		int xTotal = 0;
		int yTotal = 0;

		for (int i = 0; i < webCamTex.width; i += PIXEL_INCREMENT) {
			for (int j = 0; j < webCamTex.height; j += PIXEL_INCREMENT) {
				Color webCamCol = webCamTex.GetPixel(i, j);
				float lightness = (webCamCol.r + webCamCol.g + webCamCol.b) / 3;
				Debug.Log ("Lightness: " + lightness + "  GrayScale: " + webCamCol.grayscale);
				if (lightness > saturationThreshold) {
					numberPixelsAboveThreshold++;
					xTotal += i;
					yTotal += j;
//					webCamTex.SetPixel(
				} 
			}
		}

		#if TEST
		testTexture.material.SetTexture ("_WebCamTexture", webCamTex);

//		outputTexture.material.SetTexture ("_WebCamTexture", webCamTex);
		#endif

//		if (numberPixelsAboveThreshold > 0) {
//			int averageX = Mathf.RoundToInt (xTotal / numberPixelsAboveThreshold);
//			int averageY = Mathf.RoundToInt (yTotal / numberPixelsAboveThreshold);
//
//			return GetPositionFromWebCamData( new Vector2 (averageX, averageY));
//		}
	}

	Vector3 GetPositionFromWebCamData(Vector2 webCamCoords){

		float xPos = Mathf.Clamp01((webCamCoords.x - cameraWidthBuffer) / (webCamTex.width - cameraWidthBuffer));
		float yPos = Mathf.Clamp01((webCamCoords.y - cameraHeightBuffer) / (webCamTex.height - cameraHeightBuffer));

		float xRange = MAX_X - MIN_X;
		float yRange = MAX_Y - MIN_Y;

		xPos = (xPos * xRange) + MIN_X;
		yPos = (yPos * yRange) + MIN_Y;

		return new Vector3 (-xPos, yPos, 0);
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
}
