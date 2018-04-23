using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerTrigger : OWTrigger {

	public StringVariable battleUuid;
	public ScrObjLibraryVariable enemyLibrary;
	public BattleEntry towerBattle;
	public IntVariable towerLevel;


	public override void Trigger() {
		towerBattle.entryName = "Level " + towerLevel;
		FillQuota(30+30*towerLevel.value);
		towerLevel.value++;

		Debug.Log("Start battle: "+ towerBattle.entryName);
		battleUuid.value = towerBattle.uuid;
		startEvent.Invoke();
	}

	
	/// <summary>
	/// Picks enemies until the quota is filled. Avoids overfilling the quota with more than 100.
	/// </summary>
	/// <param name="quota"></param>
	private void FillQuota(int quota) {
		towerBattle.enemyTypes = new List<EnemyEntry>();
		towerBattle.numberOfEnemies = 0;
		EnemyEntry ee;
		int hp;
		while (quota > 0) {
			ee = (EnemyEntry)enemyLibrary.GetRandomEntry();
			hp = ee.maxhp;
			if (hp < quota+100) {
				towerBattle.enemyTypes.Add(ee);
				towerBattle.numberOfEnemies++;
				quota -= hp;
			}
		}
	}
}
