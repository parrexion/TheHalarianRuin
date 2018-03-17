using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class VarTrigger : MonoBehaviour {


	/// <summary>
	/// Triggers the variable update depending on the trigger type.
	/// </summary>
	public abstract void Trigger();

}
