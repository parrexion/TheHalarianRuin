using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which is used to add items and module to the inventory.
/// </summary>
public class AddItemDB : MonoBehaviour {

	public InventoryHandler invItemHandler;
	public InventoryHandler invModuleHandler;
	public ScrObjLibraryVariable itemLibrary;
	public ScrObjLibraryVariable moduleLibrary;

	public UnityEvent inventoryChanged;


	/// <summary>
	/// Adds the specified module to the inventory.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="equip"></param>
	public void AddSpecificModule(int id, bool equip){
		Module module = (Module)moduleLibrary.GetEntryByIndex(id);
		AddModule(module, equip);
	}

	/// <summary>
	/// Adds the specified item to the inventory.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="equip"></param>
	public void AddSpecificEquip(int id, bool equip){
		ItemEquip item = (ItemEquip)itemLibrary.GetEntryByIndex(id);
		AddItem(item, equip);
	}

	/// <summary>
	/// Adds a random module to the bag.
	/// </summary>
	public void AddRandomModule() {
		Module module = (Module)moduleLibrary.GetRandomEntry();
		AddModule(module, false);
	}

	/// <summary>
	/// Adds a random item to the bag.
	/// </summary>
	public void AddRandomEquip() {
		ItemEquip item = (ItemEquip)itemLibrary.GetRandomEntry();
		AddItem(item, false);
	}

	/// <summary>
	/// Adds the item to inventory.
	/// </summary>
	/// <param name="item"></param>
	/// <param name="equip"></param>
	void AddItem(ItemEquip item, bool equip) {
		bool added;
		Debug.Log("I'm Adding an item!");
		if (equip)
			added = invItemHandler.AddEquip(item);
		else
			added = invItemHandler.AddBag(item);
		if (added) {
			inventoryChanged.Invoke();
		}
		else {
			Debug.Log("No room left to add item.");
		}
	}

	/// <summary>
	/// Adds the module to the inventory.
	/// </summary>
	/// <param name="module"></param>
	/// <param name="equip"></param>
	void AddModule(Module module, bool equip) {
		bool added;
		if (equip)
			added = invModuleHandler.AddEquip(module);
		else
			added = invModuleHandler.AddBag(module);
		if (added) {
			inventoryChanged.Invoke();
		}
		else {
			Debug.Log("No room left to add module.");
		}
	}
}
