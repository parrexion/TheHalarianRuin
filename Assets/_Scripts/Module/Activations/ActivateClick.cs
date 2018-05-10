using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/Activations/Click")]
public class ActivateClick : ModuleActivation {

    public override bool CanActivate(Module values, MouseInformation info) {
        
		if (info.holding || info.holdDuration > values.holdMax)
			return false;

		if (!info.clicked)
			return false;
		
		float dist = info.GetInternalDistance();
		if (dist > values.area) {
			return false;
		}

		return true;
    }
}
