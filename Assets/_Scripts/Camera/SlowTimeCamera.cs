using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRatio))]
public class SlowTimeCamera : MonoBehaviour {

	[Header("References")]
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;

	[Header("Slow Time Effect")]
	public bool isSoldierCamera;
	public Color colorTint;

	private CameraRatio cameraRatio;
	private Texture2D effectTexture;
	private Rect cameraRect;
	private bool initialized = false;


	void Start () {
		cameraRatio = GetComponent<CameraRatio>();

		StartCoroutine(InitializeRects());
	}

	IEnumerator InitializeRects() {
		while (!cameraRatio.initialized){
			yield return null;
		}

		Rect tempRect = GetComponent<Camera>().rect;
		cameraRect = new Rect(tempRect.x * Screen.width, (1-tempRect.y-tempRect.height) * Screen.height, tempRect.width * Screen.width, tempRect.height * Screen.height);

		effectTexture = new Texture2D(1,1);
        effectTexture.SetPixel(0,0,colorTint);
        effectTexture.Apply();

		initialized = true;

		yield break;
	}
	
	/// <summary>
	/// Renders the slow time tint if the side is currently slowed down.
	/// </summary>
    void OnGUI() {

		if (!initialized)
			return;

		if (!useSlowTime.value || slowSoldierSide.value != isSoldierCamera)
			return;

        GUI.DrawTexture(cameraRect,effectTexture);
    }
}
