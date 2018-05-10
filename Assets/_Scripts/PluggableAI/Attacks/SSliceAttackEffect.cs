using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AiAttacks/Project/SSliceAttack")]
public class SSliceAttackEffect : AttackEffect {


	public override void Attack(StateController controller, AttackScript attackScript){

		if (!(controller is SStateController)) {
			Debug.LogError("Wrong controller user!");
			return;
		}

		Vector2 playerPos = controller.sPlayer.position;
		var shotTransform = Instantiate(attackScript.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		shotTransform.position = controller.sPlayer.position;

		if (setRotation) {
			shotTransform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = attackScript.multihit;
		projectile.SetDamage(attackScript.damage, 0, 1);

		attackScript.bgui.effectList.Add(projectile);
	}
}
