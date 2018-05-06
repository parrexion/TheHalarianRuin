using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which is the base for all the effects created by the module attacks.
/// </summary>
public abstract class AttackEffect : ScriptableObject {

	public bool setRotation = true;
	public bool targeting = false;

	/// <summary>
	/// Trigger the module effect.
	/// </summary>
	/// <param name="values"></param>
	/// <param name="info"></param>
	/// <returns></returns>
	abstract public void Attack(StateController controller, AttackScript attackScript);
}
