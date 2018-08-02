﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/True")]
public class TrueDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {
		return true;
	}
		
}
