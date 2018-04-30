using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains all information used by a module.
/// Contains activation requirements, effects and stats.
/// </summary>
[CreateAssetMenu (menuName = "LibraryEntries/Module")]
public class Module : ItemEntry {

	public Sprite chargingIcon;
	public List<ModuleActivation> activations = new List<ModuleActivation>();
	public List<ModuleEffect> effects = new List<ModuleEffect>();
	public ModuleValues values;


	/// <summary>
	/// Checks if all activation requirements are fullfilled.
	/// </summary>
	/// <param name="info"></param>
	/// <returns></returns>
	public bool CanActivate(MouseInformation info) {

		for (int i = 0; i < activations.Count; i++) {
			if (!activations[i].CanActivate(values, info))
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
			effects[i].Use(values, attackValue, info);
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
		values.ResetValues();
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
		values.CopyValues(module.values);
	}
}
