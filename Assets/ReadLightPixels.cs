using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadLightPixels : MonoBehaviour {

	//Reading shader texture
	[SerializeField] int texReadIncrement = 4;
	WebCamShader webCamShaderScript;

	Texture2D renderedTex = null;

	Color checkCol;

	// Use this for initialization
	void Start () {
		webCamShaderScript = GetComponent<WebCamShader> ();
		checkCol = webCamShaderScript.renderLightColor;
	}
	
	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		Graphics.Blit (source, destination);

		if (renderedTex == null) {
			renderedTex = new Texture2D (source.width, source.height);
		}

		renderedTex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
		renderedTex.Apply ();

		for (int i = 0; i < renderedTex.width; i += texReadIncrement) {
			for (int j = 0; j < renderedTex.height; j += texReadIncrement) {
				Color col = renderedTex.GetPixel (i, j);
				if(Mathf.Abs(col.r - checkCol.r) < 0.01f && Mathf.Abs(col.g - checkCol.g) < 0.01f && Mathf.Abs(col.b - checkCol.b) < 0.01f){
//					Debug.Log ("X: " + i + "  Y: " + j);
				}
			}
		}

	}
}
