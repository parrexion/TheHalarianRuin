using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBattleSpawner : MonoBehaviour {

	public BattleEntry[] battles;
	public BoxCollider2D spawnArea;
	public Transform triggerPrefab;
	public IntVariable fadeInTime;
	public int spawnAmount = 1;

	private float spawnWidth;
	private float spawnHeight;
	private float triggerWidth;
	private float triggerHeight;


	private void Start () {
		spawnWidth = spawnArea.size.x * 0.5f;
		spawnHeight = spawnArea.size.y * 0.5f;
		triggerWidth = triggerPrefab.localScale.x * 0.5f;
		triggerHeight = triggerPrefab.localScale.y * 0.5f;

		for (int i = 0; i < spawnAmount; i++) {
			SpawnBattle(i);
		}
	}

	private void SpawnBattle(int index) {
		float x = spawnArea.transform.position.x + Random.Range(-spawnWidth+triggerWidth,spawnWidth-triggerWidth);
		float y = spawnArea.transform.position.y + Random.Range(-spawnHeight+triggerHeight, spawnHeight-triggerHeight);

		Transform trigger = Instantiate(triggerPrefab, transform);
		trigger.name = "RandomBattle " + index;
		trigger.position = new Vector3(x,y,0);
		trigger.localScale = triggerPrefab.localScale;

		UUID uuid = trigger.GetComponent<UUID>();
		uuid.uuid = "-1";

		BattleTrigger bTrigger = trigger.GetComponent<BattleTrigger>();
		bTrigger.battle = battles[Random.Range(0,battles.Length)];
		bTrigger.alwaysActive = true;
		bTrigger.visible = true;
		bTrigger.fadeInTime = fadeInTime.value;
		bTrigger.spawning = true;

		trigger.gameObject.SetActive(true);
	}

}
