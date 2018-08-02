using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorBobber : MonoBehaviour {

	public float cycleLength;
	public float dipDuration;
	public float dipLength;

	private Image cursor;
	private Vector3 startPosition;
	private Vector3 dipPosition;


	private void Start () {
		cursor = GetComponent<Image>();
		startPosition = transform.localPosition;
		dipPosition = new Vector3(startPosition.x, startPosition.y - dipLength, startPosition.z);
		StartCoroutine(Bob(cycleLength - dipDuration, dipDuration));
	}

	IEnumerator Bob(float upLength, float downLength) {
		while(true) {
			transform.localPosition = startPosition;
			yield return new WaitForSeconds(upLength);
			transform.localPosition = dipPosition;
			yield return new WaitForSeconds(downLength);
		}
	}

	public void ShowCursor(bool visible) {
		if (cursor == null)
			cursor = GetComponent<Image>();
			
		cursor.enabled = visible;
	}

}
