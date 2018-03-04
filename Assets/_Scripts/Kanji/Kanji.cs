using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains all information used by a kanji.
/// Contains activation requirements, effects and stats.
/// </summary>
[CreateAssetMenu (menuName = "LibraryEntries/Kanji")]
public class Kanji : ItemEntry {

	public List<KanjiActivation> activations = new List<KanjiActivation>();
	public List<KanjiEffect> effects = new List<KanjiEffect>();
	public KanjiValues values;


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
	/// Creates the effect of the kanji.
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

		activations = new List<KanjiActivation>();
		effects = new List<KanjiEffect>();
		values.ResetValues();
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		Kanji kanji = (Kanji)other;

		activations = new List<KanjiActivation>();
		for (int i = 0; i < kanji.activations.Count; i++) {
			activations.Add(kanji.activations[i]);
		}
		effects = new List<KanjiEffect>();
		for (int i = 0; i < kanji.effects.Count; i++) {
			effects.Add(kanji.effects[i]);
		}
		values.CopyValues(kanji.values);
	}
}
