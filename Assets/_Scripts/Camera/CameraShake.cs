using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public FloatVariable shakeTime;
	public FloatVariable xMagnitude;
	public FloatVariable yMagnitude;

	private RectTransform rect;
	private Vector2 startPosition;


	private void Start() {
		rect = GetComponent<RectTransform>();
		startPosition = rect.anchoredPosition;
	}

	public void StartCameraShake() {
		StartCoroutine(Shake());
	}

	IEnumerator Shake() {
		float currentTime = 0;
		rect.anchoredPosition = startPosition;

		while(currentTime < shakeTime.value) {
			float xOffset = Random.Range(-xMagnitude.value,xMagnitude.value);
			float yOffset = Random.Range(-yMagnitude.value,yMagnitude.value);
			rect.anchoredPosition = new Vector2(startPosition.x + xOffset, startPosition.y + yOffset);

			currentTime += Time.deltaTime;
			yield return null;
		}

		rect.anchoredPosition = startPosition;
		yield break;
	}
}
