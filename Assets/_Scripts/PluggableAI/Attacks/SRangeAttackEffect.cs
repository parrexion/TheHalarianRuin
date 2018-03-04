using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AiAttacks/Project/SRangeAttack")]
public class SRangeAttackEffect : AttackEffect {


	public override void Attack(StateController controller, AttackScript attackScript){

		if (!(controller is SStateController)) {
			Debug.LogError("Wrong controller user!");
			return;
		}

		Vector2 playerPos = controller.sPlayer.position;
		var shotTransform = Instantiate(attackScript.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		shotTransform.position = controller.thisTransform.position;
		MouseInformation info = new MouseInformation();
		info.position1 = controller.thisTransform.position;
		info.setPosition2(controller.sPlayer.position);
		if (setRotation) {
			shotTransform.localRotation = Quaternion.AngleAxis(info.rotationInternal, Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = attackScript.multihit;
		projectile.lifeTime = attackScript.lifeTime;
		projectile.SetDamage(attackScript.damage, 0, 1);
		projectile.SetMovement(attackScript.speed, info.rotationInternal);

		attackScript.bgui.effectList.Add(projectile);
	}
}
