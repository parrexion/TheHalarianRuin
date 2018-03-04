using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SettingsValues : MonoBehaviour {

	[Header("Global values")]
	public IntVariable bestTowerLevel;
	public IntVariable currentTowerLevel;

	[Header("Settings")]
	public FloatVariable musicVolume;
	public FloatVariable effectVolume;


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

	public SettingsSaveClass SaveSettings() {
		SettingsSaveClass settingsSave = new SettingsSaveClass();

		settingsSave.bestLevel = Mathf.Max(settingsSave.bestLevel,currentTowerLevel.value);
		settingsSave.musicVolume = musicVolume.value;
		settingsSave.effectVolume = effectVolume.value;

		return settingsSave;
	}

	public void LoadSettings(SettingsSaveClass settingsSave) {
		bestTowerLevel.value = settingsSave.bestLevel;
		musicVolume.value = settingsSave.musicVolume;
		effectVolume.value = settingsSave.effectVolume;
	}

}



/// <summary>
/// Class containing most of the save data.
/// </summary>
public class SettingsSaveClass {
	public int bestLevel = 0;
	public float musicVolume = 0;
	public float effectVolume = 0;
}
