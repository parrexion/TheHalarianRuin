using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsPercentModifier {

	public enum Stat {ATTACK = 0, DEFENSE = 1, MAXHEALTH = 2, SATTACK = 100, SDEFENSE = 101}
	public Stat affectedStat = Stat.ATTACK;
	public float percentValue = 1;
	public float percentDiff = 0;


	public string GetEffectString() {

		if (percentDiff == 0) {
			return "";
		}

		string valueDifference = (percentDiff > 0) ? "+ " + percentDiff + "% " : "- " + Mathf.Abs(percentDiff) + "% ";
		int statIndex = (int)affectedStat;
		string selectedStat = (statIndex >= 100) ? "SOLDIER: " + ((Stat)(statIndex-100)) : affectedStat.ToString();

		return valueDifference + selectedStat;
	}
}
