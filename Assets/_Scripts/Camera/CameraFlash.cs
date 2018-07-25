using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFlash : MonoBehaviour {

	[Header("References")]
	public Image flashImage;
	public Color flashColor;

	[Header("Settings")]
	public FloatVariable flashBeforeTime;
	public FloatVariable flashAfterTime;
	

	// Use this for initialization
	void Start () {
		flashColor.a = 0;
		flashImage.color = flashColor;
	}


	public void StartScreenFlash() {
		StartCoroutine(ScreenFlash());
	}

	IEnumerator ScreenFlash() {
		float currentTime = 0;
		while (currentTime < flashBeforeTime.value) {
			flashColor.a = Mathf.Lerp(0,1,currentTime/flashBeforeTime.value);
			flashImage.color = flashColor;
			currentTime += Time.deltaTime;
			yield return null;
		}

		currentTime = 0;
		while (currentTime < flashAfterTime.value) {
			flashColor.a = Mathf.Lerp(1,0,currentTime/flashAfterTime.value);
			flashImage.color = flashColor;
			currentTime += Time.deltaTime;
			yield return null;
		}

		flashColor.a = 0;
		flashImage.color = flashColor;
		yield break;
	}
}
