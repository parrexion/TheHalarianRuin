using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerModule : MonoBehaviour {

	public bool active = true;
	public Module module;

	[HideInInspector] public Rect slotPos;
	[HideInInspector] public Rect slotFilled;

	private float currentCooldown, maxCooldown;
	private float currentCharge;
	private float tickCooldown;

	// Use this for initialization
	public void Initialize (int slot) {
		if (module == null){
			return;
		}
		
		currentCharge = module.maxCharges;
		currentCooldown = maxCooldown = module.cooldown;
		if (module.startCooldownTime > 0){
			currentCharge = 0;
			currentCooldown = maxCooldown = module.startCooldownTime;
		}

		if (currentCharge == 0) {
			active = false;
		}
	}
	
	public void LowerCooldown(float time) {
		if (module == null){
			return;
		}
		if (!active) {
			currentCooldown -= time;
			if (currentCooldown <= 0f) {
				active = true;
				currentCooldown = 0f;
				currentCharge = maxCooldown = module.maxCharges;
			}
		}
	}

	public bool CanActivate (MouseInformation info) {
		if (module == null){
			return false;
		}
		if (!active)
			return false;

		if (currentCharge <= 0)
			return false;

		return module.CanActivate(info);
	}

	public void CreateEffect(MouseInformation info, int attackValue) {
		module.CreateEffects(info, attackValue);
	}

	/// <summary>
	/// Reduces the amount of charges left in the module.
	/// If cooldown is negative then it means there is no rechages.
	/// If cooldown is 0 then no charges are removed.
	/// </summary>
	/// <param name="amount"></param>
	public void reduceCharge(float amount = 1) {
		if (module.cooldown == 0)
			return;

		currentCharge -= amount;

		if (currentCharge <= 0 && module.cooldown > 0) {
			active = false;
			currentCooldown = maxCooldown = module.cooldown;
		}
	}

	public float GetCharge() {
		if (module == null){
			return 0;
		}
		if (active)
			return (float)currentCharge/(float)module.maxCharges;
		else
			return (float)(maxCooldown-currentCooldown)/(float)maxCooldown;
	}

}
