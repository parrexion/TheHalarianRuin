using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class which handles displaying the different save files and 
/// handles the saving and loading of them.
/// </summary>
public class SaveFileController : MonoBehaviour {

	public int selectedSaveFile = -1;
	public BoolVariable isCurrentlySaving;
	public SaveFileCurrentProgress progress;

	[Header("Buttons")]
	public SaveFileButton[] saveFileButtons;
	public GameObject saveButton;
	public GameObject loadButton;

	[Header("File visuals")]
	public ScrObjLibraryVariable itemLibrary;
	public ScrObjLibraryVariable moduleLibrary;
	public Sprite normalImage;
	public Sprite selectedImage;
	public Sprite emptyEquipSlot;

	[Header("File Popup")]
	public GameObject filePopup;
	public Text popupMessage;
	private bool accessingFiles = false;

	[Header("Events")]
	public IntVariable currentSaveFileIndex;
	public UnityEvent changeMapEvent;
	public UnityEvent saveGameEvent;
	public UnityEvent loadGameEvent;


	// Use this for initialization
	private void OnEnable() {
		itemLibrary.GenerateDictionary();
		moduleLibrary.GenerateDictionary();
		SelectSaveFile(-1);
		if (progress != null)
			progress.ShowCurrentProgress();
	}

	IEnumerator WaitForLoading() {
		while(SaveController.instance == null)
			yield return null;

		SelectSaveFile(-1);
		if (progress != null)
			progress.ShowCurrentProgress();
		yield break;
	}

	/// <summary>
	/// Selects the save file and deselects the other ones.
	/// </summary>
	/// <param name="index"></param>
	public void SelectSaveFile(int index) {
		selectedSaveFile = index;
		for (int i = 0; i < saveFileButtons.Length; i++) {
			SetupSaveFile(i);
		}
		if (saveButton != null)
			saveButton.SetActive(selectedSaveFile != -1 && isCurrentlySaving.value);
		loadButton.SetActive(selectedSaveFile != -1 && !isCurrentlySaving.value);
	}

	/// <summary>
	/// Set ups the save file's information and image
	/// </summary>
	/// <param name="save"></param>
	void SetupSaveFile(int saveIndex) {
		PlayerStatsSaveClass pssc = SaveController.instance.saveFiles.playerSave[saveIndex];
		SaveFileButton button = saveFileButtons[saveIndex];

		bool newSave = (pssc.playerArea == 0);
		button.emptyFile.SetActive(newSave);
		button.saveStats.SetActive(!newSave);
		if (!isCurrentlySaving.value && newSave && selectedSaveFile == saveIndex)
			selectedSaveFile = -1;

		button.SetImage((saveIndex == selectedSaveFile) ? selectedImage : normalImage);

		button.currentChapter.text = "Ch. " + pssc.currentChapterIndex;
		button.currentArea.text = ((Constants.OverworldArea)pssc.playerArea).ToString();
		button.level.text = "Level " + new ExpLevel(pssc.expTotal).level;
		button.playTime.text = "Time: " + Constants.PlayTimeFromInt(pssc.playedSeconds, false);
		Sprite icon;
		for (int i = 0; i < Constants.GEAR_EQUIP_SPACE; i++) {
			icon = emptyEquipSlot;
			if (!string.IsNullOrEmpty(pssc.invItemEquip.uuids[i])) {
				ItemEntry item = (ItemEntry)itemLibrary.GetEntry(pssc.invItemEquip.uuids[i]);
				icon = item.icon;
			}
			button.equipments[i].sprite = icon;
		}
		for (int i = 0; i < Constants.MODULE_EQUIP_SPACE; i++) {
			icon = emptyEquipSlot;
			if (!string.IsNullOrEmpty(pssc.invModuleEquip.uuids[i])) {
				Module module = (Module)moduleLibrary.GetEntry(pssc.invModuleEquip.uuids[i]);
				icon = module.icon;
			}
			button.modules[i].sprite = icon;
		}
	}

	/// <summary>
	/// Saves the information from the current progress into the save file.
	/// </summary>
	/// <param name="index"></param>
	public void SaveSaveFile() {
		if (selectedSaveFile == -1 || accessingFiles)
			return;
		
		Debug.Log("Saving file: " + selectedSaveFile);
		accessingFiles = true;
		StartCoroutine(FilePopup("Saving...", true));
	}

	/// <summary>
	/// Called when the saving is complete.
	/// </summary>
	public void SaveComplete() {
		SetupSaveFile(currentSaveFileIndex.value);
		Debug.Log("Finished saving the file  " + currentSaveFileIndex.value);
		accessingFiles = false;
	}

	/// <summary>
	/// Loads the information from the save file into the current progress.
	/// </summary>
	/// <param name="index"></param>
	public void LoadSaveFile() {
		if (selectedSaveFile == -1 || accessingFiles)
			return;
		
		Debug.Log("Loading file: " + selectedSaveFile);
		accessingFiles = true;
		StartCoroutine(FilePopup("Loading...", false));
	}

	/// <summary>
	/// Called when the loading is complete
	/// </summary>
	public void LoadComplete() {
		Debug.Log("Finished loading the file");
		accessingFiles = false;
	}

	/// <summary>
	/// Save/Load popup display.
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	IEnumerator FilePopup(string message, bool isSaving) {
		popupMessage.text = message;
		filePopup.SetActive(true);
		yield return new WaitForSecondsRealtime(1f);

		currentSaveFileIndex.value = selectedSaveFile;
		if (isSaving)
			saveGameEvent.Invoke();
		else
			loadGameEvent.Invoke();

		while (accessingFiles) {
			yield return new WaitForSecondsRealtime(0.1f);
		}

		SelectSaveFile(-1);
		popupMessage.text = "Completed  " + message;
		yield return new WaitForSecondsRealtime(1f);

		filePopup.SetActive(false);
		if (!isSaving)
			changeMapEvent.Invoke();
		yield break;
	}
}
