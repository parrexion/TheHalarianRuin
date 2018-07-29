using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleTrigger : OWTrigger {

	public BattleEntry battle;
	public StringVariable battleUuid;
	public float fadeInTime;

	public bool spawning;
	private float currentTime;
	private Color invisibleColor = new Color(1,1,1,0);


	private void Start() {
		currentTime = 0;
		spawning = (fadeInTime != 0);
		if (spawning) {
			sprite.color = invisibleColor;
			BoxCollider2D boxy = GetComponent<BoxCollider2D>();
			boxy.enabled = false;
		}
	}

	private void Update() {
		if (!spawning || paused.value)
			return;

		currentTime += Time.deltaTime;
		float diff = currentTime / fadeInTime * 0.5f;
		sprite.color = Color.Lerp(invisibleColor, Color.white, diff);
		if (currentTime > fadeInTime) {
			spawning = false;
			sprite.color = Color.white;
			BoxCollider2D boxy = GetComponent<BoxCollider2D>();
			boxy.enabled = true;
		}
	}

	public override void Trigger() {
		if (spawning)
			return;

		paused.value = true;
		battleUuid.value = battle.uuid;

		Deactivate();
		TriggerOtherTriggers();
		startEvent.Invoke();
	}
}
