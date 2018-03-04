using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

	[HideInInspector] public BattleGUIController bgui;
	public AttackEffect[] projectileEffect;
	public Transform projectile;
	public int damage;
	public float lifeTime;
	public Vector2 speed;
	public bool multihit = true;


	void Start() {
		bgui = MainControllerScript.instance.battleGUI;
	}

	/// <summary>
	/// Call this to 
	/// Implement this 
	/// </summary>
	/// <param name="controller"></param>
	public void Attack(StateController controller) {
		for (int i = 0; i < projectileEffect.Length; i++) {
			projectileEffect[i].Attack(controller, this);
		}
	}
}
