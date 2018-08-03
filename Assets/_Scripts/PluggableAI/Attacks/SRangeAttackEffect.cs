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
		//Aim position
		if (targeting) {
			info.setPosition2(controller.sPlayer.position);
		}
		else {
			Vector2 aimPosition = new Vector2(Constants.SOLDIER_START_X, Constants.SOLDIER_START_Y);
			info.setPosition2(aimPosition);
		}
		if (setRotation) {
			shotTransform.localRotation = Quaternion.AngleAxis(info.rotationInternal, Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = attackScript.multihit;
		projectile.SetDamage(attackScript.damage, 0, 1);
		projectile.impactSound = controller.values.attackImpactSfx;
		projectile.SetMovement(attackScript.speed, info.rotationInternal);

		attackScript.bgui.effectList.Add(projectile);

		if (controller.values.attackActivateSfx != null) {
			controller.currentSfx.value.Enqueue(controller.values.attackActivateSfx.clip);
			controller.playSfxEvent.Invoke();
		}
	}
}
