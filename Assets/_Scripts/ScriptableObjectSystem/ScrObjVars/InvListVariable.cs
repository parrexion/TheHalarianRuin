using UnityEngine;

/// <summary>
/// Representative list of inventory items.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/Inventory List Variable")]
public class InvListVariable : ScriptableObject {

	public ItemEntry[] values;

	
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
		Debug.Log("leangth:  " + values.Length);
		for (int i = 0; i < values.Length; i++) {
			Debug.Log(i + ": res:  " + values[i]);
			if (values[i].uuid == uuid){
				return values[i];
			}
		}

		Debug.Log("Failed to find itemEntry with uuid: " + uuid);
		return null;
	}

	//Saving and loading

	/// <summary>
	/// Generates a list of uuids in order to save the list to file.
	/// </summary>
	/// <returns></returns>
	public SaveListUuid GenerateSaveData() {
		SaveListUuid saveData = new SaveListUuid();
		int length = values.Length;
		saveData.size = length;
		saveData.uuids = new string[length];

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
		Debug.Log("Loaded the module list.");
	}
}
