using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileCurrentProgress : MonoBehaviour {

	public Sprite emptyEquipSlot;
	public SaveFileButton saveButton;
	
	[Header("Current Progress")]
	public StringVariable currentChapter;
	public AreaIntVariable playerArea;
	public IntVariable playerLevel;
	public IntVariable playTime;
	public InvListVariable equipItems;
	public InvListVariable modules;


	private void OnEnable() {
		ShowCurrentProgress();
	}

	/// <summary>
	/// Displays the player's current progress compared to the save files.
	/// </summary>
	public void ShowCurrentProgress() {
		saveButton.currentChapter.text = "Ch. " + currentChapter.value;
		saveButton.currentArea.text = playerArea.AreaName();
		saveButton.level.text = "Level " + playerLevel.value;
		saveButton.playTime.text = "Time: " + Constants.PlayTimeFromInt(playTime.value, false);
		for (int i = 0; i < Constants.GEAR_EQUIP_SPACE; i++) {
			saveButton.equipments[i].sprite = (equipItems.values[i] != null) ? equipItems.values[i].icon : emptyEquipSlot;
		}
		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			saveButton.modules[i].sprite = (modules.values[i] != null) ? modules.values[i].icon : emptyEquipSlot;
		}
	}
}
