using UnityEngine;

/// <summary>
/// Representative list of inventory items.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/Inventory List Variable")]
public class InvListVariable : ScriptableObject {

	public ItemEntry[] values;

	
	/// <summary>
	/// Looks through and adds the item to the first available slot in the 
	/// inventory list. Returns a bool indicating if there was room for the item.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool AddItem(ItemEntry item) {
		for (int i = 0; i < values.Length; i++) {
			if (values[i] == null){
				values[i] = item;
				Debug.Log("Added the item to index " + i);
				return true;
			}
		}
		Debug.LogWarning("Failed to add the item. No more room!");
		return false;
	}

	/// <summary>
	/// Returns the module representation with activations, projectiles and values etc...
	/// Takes the index for the position in the module list.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public ItemEntry GetItem(int index) {
		if (index >= values.Length || index < 0) {
			Debug.Log("Index is out of bounds! " + index);
			return null;
		}

		return values[index];
	}
	
	/// <summary>
	/// Returns the module representation with activations, projectiles and values etc...
	/// Searches on the name of the module.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public ItemEntry GetItemByUuid(string uuid) {
		for (int i = 0; i < values.Length; i++) {
			if (values[i].uuid == uuid){
				return values[i];
			}
		}

		Debug.Log("Failed to find itemEntry with uuid: " + uuid);
		return null;
	}

	public void Reset() {
		for (int i = 0; i < values.Length; i++) {
			values[i] = null;
		}
		Debug.Log("Reset the inventory");
	}


	//Saving and loading

	/// <summary>
	/// Generates a list of uuids in order to save the list to file.
	/// </summary>
	/// <returns></returns>
	public SaveListUuid GenerateSaveData() {
		int length = values.Length;
		SaveListUuid saveData = new SaveListUuid(length);

		for (int i = 0; i < length; i++) {
			saveData.uuids[i] = (values[i] != null) ? values[i].uuid : "";
		}

		return saveData;
	}

	/// <summary>
	/// Loads a list of uuids into the module list.
	/// </summary>
	/// <param name="saveData"></param>
	public void LoadItemData(SaveListUuid saveData, ScrObjLibraryVariable itemLibrary) {
		if (values.Length != saveData.size)
			Debug.LogWarning("Something is wrong with the size of the module list.");
		for (int i = 0; i < saveData.size; i++) {
			values[i] = string.IsNullOrEmpty(saveData.uuids[i]) ? null : (ItemEntry)itemLibrary.GetEntry(saveData.uuids[i]);
		}
	}
}
