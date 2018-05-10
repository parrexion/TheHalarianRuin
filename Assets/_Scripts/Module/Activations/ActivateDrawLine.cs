using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/Activations/DrawLine")]
public class ActivateDrawLine : ModuleActivation {

	public float lowAngle;
	public float highAngle;
	public bool bothDirections = false;

    public override bool CanActivate(Module values, MouseInformation info) {
        
		if (info.holding || info.holdDuration > 0.5f)
			return false;

		if (!info.clicked)
			return false;
		
		float dist = info.GetInternalDistance();
		if (dist < values.area) {
			return false;
		}

		float lCone = Mathf.PI * lowAngle / 180.0f;
		float hCone = Mathf.PI * highAngle / 180.0f;

		if (lCone < info.rotationInternal && info.rotationInternal < hCone) {
			return true;
		}

		if (bothDirections) {
			lCone = lCone - (Mathf.PI * Mathf.Sign(lCone));
			hCone = hCone - (Mathf.PI * Mathf.Sign(hCone));

			if (lCone < info.rotationInternal || info.rotationInternal < hCone) {
				return true;
			}
		}

		return false;
    }
}
