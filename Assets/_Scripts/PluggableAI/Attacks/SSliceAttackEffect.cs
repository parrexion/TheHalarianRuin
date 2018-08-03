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

		if (targeting)
			shotTransform.position = controller.sPlayer.position;
		else
			shotTransform.position = new Vector3(Constants.SOLDIER_START_X, Constants.SOLDIER_START_Y, 0);

		if (setRotation) {
			shotTransform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = attackScript.multihit;
		projectile.SetDamage(attackScript.damage, 0, 1);
		projectile.impactSound = controller.values.attackImpactSfx;

		attackScript.bgui.effectList.Add(projectile);

		if (controller.values.attackActivateSfx != null) {
			controller.currentSfx.value.Enqueue(controller.values.attackActivateSfx.clip);
			controller.playSfxEvent.Invoke();
		}
	}
}
