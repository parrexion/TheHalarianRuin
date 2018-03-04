using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player control class for the battle tower game mode.
/// </summary>
public class TowerPlayerController : MonoBehaviour {

	public BoolVariable paused;
	private MoveHomingNoLimit moveToPosition;
	private Camera cam;

	// Use this for initialization
	void Start () {
		moveToPosition = GetComponent<MoveHomingNoLimit>();
		cam = Camera.main;
		paused.value = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (paused.value)
			return;

		if (Input.GetMouseButton(1)) {
			moveToPosition.moveToPosition = cam.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
