using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBox : MonoBehaviour {

	public CameraRatio ratio;
	public Transform objectToFollow;
	public float top;
	public float left;
	public float right;
	public float bottom;

	Vector3 position;
	float xpos, ypos;

	//Offsets
	private float xOffset;
	private float yOffset;
	

	private void OnEnable() {
		Camera camera = GetComponent<Camera>();
		xOffset = camera.orthographicSize * camera.rect.width;
		yOffset = camera.orthographicSize * camera.rect.height;
	}

	// Update is called once per frame
	void Update () {
		position = objectToFollow.position;
		xpos = position.x;
		ypos = position.y;

		if (xpos < left + xOffset) {
			xpos = left + xOffset;
		}
		else if (xpos > right - xOffset) {
			xpos = right - xOffset;
		}

		if (ypos > top - yOffset) {
			ypos = top - yOffset;
		}
		else if (ypos < bottom + yOffset) {
			ypos = bottom + yOffset;
		}

		transform.position = new Vector3(xpos,ypos,transform.localPosition.z);
	}
}
