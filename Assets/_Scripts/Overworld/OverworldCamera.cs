using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour {

	public CameraConstants constants;
	public Camera followCamera;
	public Camera lockCamera;
	public OutsidePlayerController playerController;
	public AreaIntVariable playerArea;
	public IntVariable playerRoomNumber;


	// Use this for initialization
	void Start () {
		SetCameraInfo(constants.GetValues((Constants.OverworldArea)playerArea.value, (Constants.RoomNumber)playerRoomNumber.value));
	}


	void SetCameraInfo(CameraValues values) {
		Camera cam;
		if (values.stationary) {
			cam = lockCamera;
			followCamera.enabled = false;
			lockCamera.enabled = true;
			cam.orthographicSize = values.size;
			cam.transform.position = new Vector3(values.position.x, values.position.y, -10);
			Debug.Log("SET THE LOCK CAMERA");
		}
		else {
			cam = followCamera;
			followCamera.enabled = true;
			lockCamera.enabled = false;
			cam.orthographicSize = constants.defaultCamSize;
			CameraBox box = cam.GetComponent<CameraBox>();
			box.top = values.cameraBox.yMin;
			box.bottom = values.cameraBox.yMax;
			box.left = values.cameraBox.xMin;
			box.right = values.cameraBox.xMax;
			Debug.Log("SET THE FOLLOW CAMERA");
		}

		playerController.cam = cam;
	}
}
