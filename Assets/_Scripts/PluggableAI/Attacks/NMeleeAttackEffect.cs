using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AiAttacks/Projectile/NMeleeAttack")]
public class NMeleeAttackEffect : AttackEffect {


	public override void Attack(StateController controller, AttackScript attackScript) {

		if (!(controller is AStateController)){
			Debug.LogError("Wrong controller user!");
			return;
		}

		AStateController ncon = (AStateController)controller;

		var shotTransform = Instantiate(attackScript.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		shotTransform.position = ncon.thisTransform.position;
		MouseInformation info = new MouseInformation();
		info.position1 = controller.thisTransform.position;
		info.setPosition2(controller.aPlayer.position);
		if (setRotation) {
			float rotation = info.rotationInternal*180/Mathf.PI;
			shotTransform.localRotation = Quaternion.AngleAxis(rotation,Vector3.forward);
		}

		projectile.isEnemy = true;
		projectile.multiHit = attackScript.multihit;
		projectile.SetDamage(attackScript.damage, 0, 1);
		projectile.impactSound = controller.values.attackImpactSfx;
		projectile.SetMovement(attackScript.speed, info.rotationInternal);

		attackScript.bgui.effectList.Add(projectile);

		if (controller.values.attackActivateSfx != null) {
			controller.currentSfx.value = controller.values.attackActivateSfx.clip;
			controller.playSfxEvent.Invoke();
		}
	}
}
