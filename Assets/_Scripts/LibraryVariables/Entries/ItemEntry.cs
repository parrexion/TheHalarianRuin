using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntry : ScrObjLibraryEntry {

	public EquipType equipType;
	public Sprite icon = null;
	public Color tintColor = Color.white;
	public int cost = 0;


	/// <summary>
	/// Resets the values to default.
	/// </summary>
	public override void ResetValues() {
		base.ResetValues();

		equipType = EquipType.WILD;
		icon = null;
		tintColor = Color.white;
		cost = 0;
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		ItemEntry item = (ItemEntry)other;

		equipType = item.equipType;
		icon = item.icon;
		tintColor = item.tintColor;
		cost = item.cost;
	}
}
