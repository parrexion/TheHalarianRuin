using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour {
	
	public Vector2 aspectRatio = new Vector2(16f,7f);
	public bool initialized = false;

	// Use this for initialization
	void Awake () {

		Camera camera = GetComponent<Camera>();

		// float targetAspect = aspectRatio.y / aspectRatio.x;
		// float screenAspect = (float)Screen.height / (float)Screen.width;
		// float scaleheight = targetAspect / screenAspect;
		// float windowDiff = Screen.width * ( screenAspect - targetAspect);

		// // Debug.Log(string.Format("T: {0}, W: {1}, S: {2}, D: {3}", targetAspect, screenAspect, scaleheight, windowDiff));
		// // Debug.Log("W: " + Screen.width + ", H: " + Screen.height);

		// Rect rect = camera.rect;
		// float height = rect.height;
		// float width = rect.width;
		// if (scaleheight < 1.0f) {
		// 	// Debug.Log("Rect: " + rect);
		// 	// Add letterbox
		// 	rect.width = width * 1.0f;
		// 	rect.height = height * scaleheight;
		// 	rect.x = rect.x + 0;
		// 	// print("Rect: " + rect.y);
		// 	// rect.y = rect.y + rect.height * ((1.0f - scaleheight) / 2.0f + windowDiff / (2.0f * Screen.width));
		// 	rect.y = rect.y + height * (windowDiff / (2.0f * Screen.height));
		// 	// Debug.Log("Res: " + (windowDiff / (2.0f * Screen.height)));
		// 	// print("Rect: " + rect.y);
		// }
		// // else {
		// // 	// Add pillarbox
		// // 	float scalewidth = 1.0f / scaleheight;
		// // 	rect.width = rect.width * scalewidth;
		// // 	rect.height = rect.height * 1.0f;
		// // 	rect.x = rect.x + rect.width * (1.0f - scalewidth) / 2.0f;
		// // 	rect.y = rect.y + 0;
		// // }



		float p2 = (float)Constants.SCREEN_HEIGHT * (float)Screen.width/(float)Constants.SCREEN_WIDTH;
		float borderAdd = ((float)Screen.height - p2) * 0.5f / Screen.height;
		float scaleheight = (1 - 2*borderAdd);
		
		Rect rect = camera.rect;
		float height = rect.height;
		float width = rect.width;

		// rect.width = width * 1.0f;
		rect.height = height * scaleheight;
		// rect.x = rect.x + 0;
		rect.y = rect.y * scaleheight + borderAdd;

		camera.rect = rect;
		initialized = true;
	}
	
}
