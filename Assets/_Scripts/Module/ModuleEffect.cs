using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which is the base for all the effects created by the module attacks.
/// </summary>
public abstract class ModuleEffect : ScriptableObject {

	public bool setRotation = true;
	public bool placeInMiddle = true;

	/// <summary>
	/// Trigger the module effect.
	/// </summary>
	/// <param name="values"></param>
	/// <param name="info"></param>
	/// <returns></returns>
	abstract public bool Use(ModuleValues values, int attackValue, MouseInformation info);
}
