using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntry : ScrObjLibraryEntry {

	public enum ItemType {MODULE,EQUIP,MISC,DESTROY,SHOP}
	public ItemType item_type = ItemType.EQUIP;
	public Sprite icon = null;
	public Color tintColor = Color.white;
	public int moneyValue = 0;


	/// <summary>
	/// Resets the values to default.
	/// </summary>
	public override void ResetValues() {
		base.ResetValues();

		item_type = ItemType.EQUIP;
		icon = null;
		tintColor = Color.white;
		moneyValue = 0;
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		ItemEntry item = (ItemEntry)other;

		item_type = item.item_type;
		icon = item.icon;
		tintColor = item.tintColor;
		moneyValue = item.moneyValue;
	}
}
