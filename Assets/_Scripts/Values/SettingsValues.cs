using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.Events;

public class SettingsValues : MonoBehaviour {
	
#region Singleton
	private static SettingsValues instance;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
#endregion

	public IntVariable currentSaveFileIndex;

	[Header("Settings")]
	public FloatVariable musicVolume;
	public FloatVariable effectVolume;

	[Header("Events")]
	public UnityEvent saveCheckEvent;
	public UnityEvent loadCheckEvent;


	/// <summary>
	/// Saves the settings to the data in the SaveController for saving.
	/// </summary>
	public void SaveSettings() {
		SettingsSaveClass settingsSave = new SettingsSaveClass();

		settingsSave.musicVolume = musicVolume.value;
		settingsSave.effectVolume = effectVolume.value;

		SaveController.instance.saveFiles.settingsSave[currentSaveFileIndex.value] = settingsSave;
		saveCheckEvent.Invoke();
		Debug.Log("SAVED");
	}

	/// <summary>
	/// Loads the settings from the data loaded by the SaveController.
	/// </summary>
	public void LoadSettings() {
		SettingsSaveClass settingsSave = SaveController.instance.saveFiles.settingsSave[currentSaveFileIndex.value];
		
		musicVolume.value = settingsSave.musicVolume;
		effectVolume.value = settingsSave.effectVolume;

		loadCheckEvent.Invoke();
		Debug.Log("LOADED");
	}

}



/// <summary>
/// Class containing most of the save data.
/// </summary>
public class SettingsSaveClass {
	public float musicVolume = 0;
	public float effectVolume = 0;
}
