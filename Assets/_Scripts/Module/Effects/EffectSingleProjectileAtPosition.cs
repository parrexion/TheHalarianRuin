using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Module/Effect/Projectile/SingleAtPosition")]
public class EffectSingleProjectileAtPosition : ModuleEffect {

    public override bool Use(Module values, int attackValue, MouseInformation info) {
		
		var shotTransform = Instantiate(values.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		if (placeInMiddle) {
			Vector3 pos = new Vector3(info.position1.x+info.distX*0.5f,info.position1.y+info.distY*0.5f,0);
			shotTransform.position = pos;
		}
		else
			shotTransform.position = info.position2;

		if (setRotation) {
			float rotation = info.rotationInternal*180/Mathf.PI;
			shotTransform.localRotation = Quaternion.AngleAxis(rotation,Vector3.forward);
		}

		projectile.isEnemy = false;
		projectile.steps = values.effectSteps;
		projectile.multiHit = values.multihit;
		projectile.SetDamage(values.damage, attackValue, values.baseDamageScale);
		projectile.impactSound = values.impactSound;
		projectile.SetMovement(values.projectileSpeed, info.rotationInternal);

		MainControllerScript.instance.battleGUI.effectList.Add(projectile);

		return true;
    }
}
