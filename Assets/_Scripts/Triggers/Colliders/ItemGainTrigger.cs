using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGainTrigger : VarTrigger {

	public InvListVariable itemList;
	public ItemEntry gainedItem;


	public override void Trigger() {
		itemList.AddItem(gainedItem);
		Debug.Log("Gained item " + gainedItem.entryName);
	}
}
