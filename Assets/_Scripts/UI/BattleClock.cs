using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleClock : MonoBehaviour {

	[Header("Game Speed")]
	public BoolVariable paused;
	public BoolVariable useSlowTime;
	public BoolVariable leftSideSlow;
	public FloatVariable changeTime;

	[Header("Clock")]
	public Transform arrow;
	private SpriteRenderer arrowRenderer;

	SpriteRenderer sprite;
	float currentTime;


	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		arrowRenderer = arrow.GetComponent<SpriteRenderer>();
		currentTime = Random.Range(0f,changeTime.value*2);
		leftSideSlow.value = (currentTime >= changeTime.value);
		if (currentTime >= changeTime.value)
			currentTime -= changeTime.value;
		sprite.color = (leftSideSlow.value) ? Color.yellow : Color.magenta;
	}
	
	// Update is called once per frame
	void Update () {
		if (paused.value || !useSlowTime.value){
			sprite.enabled = false;
			arrowRenderer.enabled = false;
			return;
		}

		sprite.enabled = true;
		arrowRenderer.enabled = true;
		currentTime += Time.deltaTime;

		if (currentTime >= changeTime.value) {
			currentTime -= changeTime.value;
			leftSideSlow.value = !leftSideSlow.value;
			sprite.color = (leftSideSlow.value) ? Color.yellow : Color.magenta;
		}

		float rotation = 180*currentTime/changeTime.value;
		if (leftSideSlow.value)
			rotation += 180;

		arrow.localRotation = Quaternion.AngleAxis(rotation,Vector3.forward);
	}
}
