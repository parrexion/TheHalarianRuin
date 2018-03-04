using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "LibraryEntries/ItemEquip")]
public class ItemEquip : ItemEntry {

	public int healthModifier;

	public int attackModifier;      // Increase/decrease in damage
	public int defenseModifier;		// Increase/decrease in defense

	public int sAttackModifier;
	public int sDefenseModifier;

	// List of additional effects
	public List<StatsPercentModifier> percentModifiers = new List<StatsPercentModifier>();

	/// <summary>
	/// Resets the values to default.
	/// </summary>
	public override void ResetValues() {
		base.ResetValues();
		healthModifier = 0;
		attackModifier = 0;
		defenseModifier = 0;
		sAttackModifier = 0;
		sDefenseModifier = 0;

		percentModifiers = new List<StatsPercentModifier>();
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		ItemEquip item = (ItemEquip)other;
		healthModifier = item.healthModifier;
		attackModifier = item.attackModifier;
		defenseModifier = item.defenseModifier;
		sAttackModifier = item.sAttackModifier;
		sDefenseModifier = item.sDefenseModifier;

		percentModifiers = new List<StatsPercentModifier>();
		for (int i = 0; i < item.percentModifiers.Count; i++) {
			percentModifiers.Add(item.percentModifiers[i]);
		}
	}
}
