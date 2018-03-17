using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolChangeTrigger : VarTrigger {

	public BoolVariable variable;
	public bool setValueTo;


	public override void Trigger() {
		variable.value = setValueTo;
		Debug.Log("Updated the bool variable " + variable.name + " ,  value: " + variable.value);
	}
}
