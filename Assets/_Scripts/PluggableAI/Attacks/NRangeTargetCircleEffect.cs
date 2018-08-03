using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AiAttacks/Project/NRangeTargetCircle")]
public class NRangeTargetCircleEffect : AttackEffect {

	public int circleProjectiles = 3;
	public float circleOffset = 1;


	public override void Attack(StateController controller, AttackScript attackScript){

		if (!(controller is AStateController)) {
			Debug.LogError("Wrong controller user!");
			return;
		}

		var shotTransform = Instantiate(attackScript.projectile) as Transform;
		Projectile projectile = shotTransform.GetComponent<Projectile>();

		if (targeting)
			shotTransform.position = controller.aPlayer.position;
		else
			shotTransform.position = controller.thisTransform.position;
		MouseInformation info = new MouseInformation();
		info.position1 = controller.thisTransform.position;
		info.setPosition2(controller.aPlayer.position);
		if (setRotation) {
			float rotation = info.rotationInternal * 180 / Mathf.PI;
			shotTransform.localRotation = Quaternion.AngleAxis(rotation, Vector3.forward);
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

		float startPoint = Random.Range(0,2*Mathf.PI);
		for (int i = 0; i < circleProjectiles; i++) {
			Projectile circleProj = Instantiate(projectile);
			Vector3 offset = circleOffset * new Vector3(Mathf.Cos(startPoint), Mathf.Sin(startPoint), 0);
			circleProj.transform.position += offset;
			startPoint += 2*Mathf.PI / circleProjectiles;
			attackScript.bgui.effectList.Add(circleProj);
		}
	}
}
