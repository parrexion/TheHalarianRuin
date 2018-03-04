using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AiAttacks/Projectile/SMeleeAttack")]
public class SMeleeAttackEffect : AttackEffect {


	public override void Attack(StateController controller, AttackScript attackScript) {

		if (!(controller is SStateController)) {
			Debug.LogError("Wrong controller user!");
			return;
		}

		SStateController scon = (SStateController)controller;

		var shotTransform = Instantiate(attackScript.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		shotTransform.position = scon.thisTransform.position;
		MouseInformation info = new MouseInformation();
		info.position1 = controller.thisTransform.position;
		info.setPosition2(controller.sPlayer.position);
		if (setRotation) {
			float rotation = info.rotationInternal * 180 / Mathf.PI;
			shotTransform.localRotation = Quaternion.AngleAxis(rotation, Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = false;
		projectile.multiHit = attackScript.multihit;
		projectile.lifeTime = attackScript.lifeTime;
		projectile.SetDamage(attackScript.damage, 0, 1);
		projectile.SetMovement(attackScript.speed, info.rotationInternal);

		attackScript.bgui.effectList.Add(projectile);
	}
}
