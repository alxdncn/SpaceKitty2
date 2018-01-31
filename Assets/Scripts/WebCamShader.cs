using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamShader : MonoBehaviour {

	WebCamTexture webCamTex;

	bool initialized = false;

	//Shader stuff
	Material renderMat;
	[SerializeField] [Range(0,1)] float saturationThreshold;
	public Color renderLightColor;

	Texture2D renderedTex = null;
	public int texReadIncrement = 4;

	Rect texRect;

	public List<Vector2> brightPixelPositions = new List<Vector2> ();

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

			if (renderedTex == null) {
				texRect = new Rect (0, 0, source.width, source.height);
				renderedTex = new Texture2D (source.width, source.height);
			}

			brightPixelPositions.Clear ();

			renderedTex.ReadPixels(texRect, 0, 0);

			Vector2 newPos = new Vector2 ();

			for (int i = texReadIncrement; i < renderedTex.width - texReadIncrement; i += texReadIncrement) {
				for (int j = texReadIncrement; j < renderedTex.height - texReadIncrement; j += texReadIncrement) {
					Color col = renderedTex.GetPixel (i, j);
					if(Mathf.Abs(col.r - renderLightColor.r) < 0.01f && Mathf.Abs(col.g - renderLightColor.g) < 0.01f && Mathf.Abs(col.b - renderLightColor.b) < 0.01f){
						newPos.x = i;
						newPos.y = j;
						brightPixelPositions.Add (newPos);
					}
				}
			}

//			Debug.Log (brightPixelPositions.Count);
		}
	}
}
