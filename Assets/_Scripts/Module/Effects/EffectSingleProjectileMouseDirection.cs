using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Module/Effect/Projectile/SingleMouseDirection")]
public class EffectSingleProjectileMouseDirection : ModuleEffect {

    public override bool Use(ModuleValues values, int attackValue, MouseInformation info) {
		
		var shotTransform = Instantiate(values.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();
		MainControllerScript.instance.battleGUI.effectList.Add(projectile);

		shotTransform.position = info.playerPosition;
		if (setRotation) {
			float rotation = info.rotationPlayer*180/Mathf.PI;
			shotTransform.localRotation = Quaternion.AngleAxis(rotation,Vector3.forward);
		}

		projectile.lifeTime = values.projectileLifetime;
		projectile.multiHit = values.multihit;
		projectile.SetDamage(values.damage, attackValue, values.baseDamageScale);
		projectile.SetMovement(values.projectileSpeed, info.rotationPlayer);

		return true;
    }
}
