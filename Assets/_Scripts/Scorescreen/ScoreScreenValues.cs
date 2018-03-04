using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreenValues : MonoBehaviour {

	public StringVariable wonBattleState;
	public FloatVariable time;
	public FloatVariable androidDamageTaken;
	public FloatVariable soldierDamageTaken;
	public IntVariable maxHealth;
	public IntVariable noEnemies;

	[Header("Loot")]
	public IntVariable exp;
	public IntVariable money;
}
