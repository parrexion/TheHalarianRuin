using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains all information used by a module.
/// Contains activation requirements, effects and stats.
/// </summary>
[CreateAssetMenu (menuName = "LibraryEntries/Module")]
public class Module : ItemEntry {

	public enum ModuleType {
		CLICK,SLASH,RISE,HOLD,DOWN,OTHER
	}

	public Sprite chargingIcon;
	public List<ModuleActivation> activations = new List<ModuleActivation>();
	public List<ModuleEffect> effects = new List<ModuleEffect>();

	[Header("Usage values")]
	public float startCooldownTime = 0f;
	public int maxCharges = 10;
	public float delay = 0.1f;
	public float cooldown = 5.0f;

	[Space(10)]

	[Header("Activation requirements")]
	public ModuleType moduleType;
	public float area = 1.0f; 
	public float holdMin = -1f;
	public float holdMax = 0.5f;

	[Space(10)]

	[Header("Projectile")]
	public Transform projectile;
	public Vector2 projectileSpeed;
	public EffectStep[] effectSteps;

	[Space(10)]

	[Header("Damage")]
	public int damage = 0;
	public float baseDamageScale = 1.0f;
	public bool multihit = true;


	/// <summary>
	/// Checks if all activation requirements are fullfilled.
	/// </summary>
	/// <param name="info"></param>
	/// <returns></returns>
	public bool CanActivate(MouseInformation info) {

		for (int i = 0; i < activations.Count; i++) {
			if (!activations[i].CanActivate(this, info))
				return false;
		}

		return true;
	}

	/// <summary>
	/// Creates the effect of the module.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="attackValue"></param>
	public void CreateEffects(MouseInformation info, int attackValue){
		for (int i = 0; i < effects.Count; i++) {
			effects[i].Use(this, attackValue, info);
		}
	}

	public override GUIContent GenerateRepresentation() {
		GUIContent content = new GUIContent();
		content.text = uuid;
		Texture2D tex;
		if (icon != null) {
			tex = icon.texture;
		}
		else {
			tex = GenerateRandomColor();
		}
		content.image = tex;
		return content;
	}

	/// <summary>
	/// Resets the values to default.
	/// </summary>
	public override void ResetValues() {
		base.ResetValues();

		activations = new List<ModuleActivation>();
		effects = new List<ModuleEffect>();
		startCooldownTime = 0;
		maxCharges = 10;
		delay = 0.1f;
		cooldown = 5f;

		moduleType = ModuleType.OTHER;
		area = 1.0f;
		holdMin = -1f;
		holdMax = 0.5f;

		projectile = null;
		projectileSpeed = Vector2.zero;
		effectSteps = new EffectStep[0];

		damage = 0;
		baseDamageScale = 1f;
		multihit = true;
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		Module module = (Module)other;

		chargingIcon = module.chargingIcon;

		activations = new List<ModuleActivation>();
		for (int i = 0; i < module.activations.Count; i++) {
			activations.Add(module.activations[i]);
		}
		effects = new List<ModuleEffect>();
		for (int i = 0; i < module.effects.Count; i++) {
			effects.Add(module.effects[i]);
		}

		startCooldownTime = module.startCooldownTime;
		maxCharges = module.maxCharges;
		delay = module.delay;
		cooldown = module.cooldown;

		moduleType = module.moduleType;
		area = module.area;
		holdMin = module.holdMin;
		holdMax = module.holdMax;

		projectile = module.projectile;
		projectileSpeed = module.projectileSpeed;
		effectSteps = module.effectSteps;

		damage = module.damage;
		baseDamageScale = module.baseDamageScale;
		multihit = module.multihit;
	}
}
