using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraValues {

	public Constants.OverworldArea area;
	public Constants.RoomNumber roomNumber;
	public bool stationary;
	public float size;
	public Vector2 position;
	public Rect cameraBox;
}

public class CameraConstants : MonoBehaviour {

	public float defaultCamSize;
	public CameraValues[] values;


	public CameraValues GetValues(Constants.OverworldArea area, Constants.RoomNumber number) {
		for (int i = 0; i < values.Length; i++) {
			if (values[i].area == area && values[i].roomNumber == number)
				return values[i];
		}
		return null;
	}
}
