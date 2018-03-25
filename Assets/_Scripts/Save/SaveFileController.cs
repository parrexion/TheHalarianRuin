using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileController : MonoBehaviour {

	public int selectedSaveFile = -1;

	[Header("Buttons")]
	public SaveFileButton[] saveFiles;
	public GameObject saveButton;

	[Header("File visuals")]
	public Sprite normalImage;
	public Sprite selectedImage;


	// Use this for initialization
	private void OnEnable() {
		SelectSaveFile(-1);
	}

	public void SelectSaveFile(int index) {
		selectedSaveFile = index;
		for (int i = 0; i < saveFiles.Length; i++) {
			SetupSaveFile(i);
		}
		saveButton.SetActive(selectedSaveFile != -1);
	}

	void SetupSaveFile(int save) {
		SaveFileButton button = saveFiles[save];
		button.SetImage((save == selectedSaveFile) ? selectedImage : normalImage);

	}
}
