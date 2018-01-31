using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamShader : MonoBehaviour {

	WebCamTexture webCamTex;

	bool initialized = false;

	//Shader stuff
	Material renderMat;
	[SerializeField] [Range(0,1)] float saturationThreshold;
	[SerializeField] Color renderLightColor;

	//Reading shader texture
	[SerializeField] int texReadIncrement = 4;

	// Use this for initialization
	void Awake () {
		webCamTex = new WebCamTexture ();
		webCamTex.Play();
		renderMat = new Material (Shader.Find ("Hidden/DrawRed"));
		renderMat.SetFloat ("_SaturationThreshold", saturationThreshold);
		renderMat.SetColor ("_RenderColor", renderLightColor);

		initialized = true;
	}

	void OnValidate(){
		if (initialized) {
			renderMat.SetFloat ("_SaturationThreshold", saturationThreshold);
			renderMat.SetColor ("_RenderColor", renderLightColor);
		}
	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (initialized) {
			renderMat.SetTexture ("_WebCamTex", webCamTex);

			Graphics.Blit (source, destination, renderMat);

			Texture renderedTex = renderMat.mainTexture;

			for (int i = 0; i < renderedTex.width; i += texReadIncrement) {
				for (int j = 0; j < renderedTex.height; j += texReadIncrement) {

				}
			}
		}
	}
}
