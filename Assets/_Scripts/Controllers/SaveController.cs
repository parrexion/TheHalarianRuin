using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SaveController : MonoBehaviour {

	public bool loadGame = true;

	[Header("Save object references")]
	public PlayerStats playerStats;
	public SettingsValues settingsValues;
	public TriggerController triggerController;

	private string playerStatsPath = "";
	private string settingsPath = "";
	private string triggerPath = "";


#region Singleton
	private static SaveController instance;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			instance = this;
			Initialize();
		}
	}
#endregion

	void Initialize() {
		settingsPath = Application.persistentDataPath+"/settingsData.xml";
		playerStatsPath = Application.persistentDataPath+"/playerData.xml";
		triggerPath = Application.persistentDataPath+"/triggerData.xml";
		Load();
	}

	/// <summary>
	/// Updates the save class and saves it to file.
	/// </summary>
	public void Save() {
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
		XmlSerializer serializer;

		if (settingsValues != null) {
			serializer = new XmlSerializer(typeof(SettingsSaveClass));

			SettingsSaveClass settingsSave = settingsValues.SaveSettings();

			using (XmlWriter xmlWriter = XmlWriter.Create(settingsPath, xmlWriterSettings)) {
				serializer.Serialize(xmlWriter, settingsSave);
			}
			// file.Close();
			Debug.Log("Successfully saved the settings!");
		}

		if (playerStats != null) {
			serializer = new XmlSerializer(typeof(PlayerStatsSaveClass));
			PlayerStatsSaveClass playerSave = playerStats.SaveStats();
			using (XmlWriter xmlWriter = XmlWriter.Create(playerStatsPath, xmlWriterSettings)) {
				serializer.Serialize(xmlWriter, playerSave);
			}
			// file.Close();
			Debug.Log("Successfully saved the player stats!");
		}

		if (triggerController != null) {
			serializer = new XmlSerializer(typeof(TriggerSaveClass));
			TriggerSaveClass triggerSave = triggerController.SaveTriggers();
			using (XmlWriter xmlWriter = XmlWriter.Create(triggerPath, xmlWriterSettings)) {
				serializer.Serialize(xmlWriter, triggerSave);
			}
			// file.Close();
			Debug.Log("Successfully saved the trigger stats!");
		}

	}

	/// <summary>
	/// Reads the save file if it exists and sets the values.
	/// </summary>
	public void Load() {
		if (!loadGame)
			return;
			
		//Settings data
		if (settingsValues != null) {
			if (settingsValues != null && File.Exists(settingsPath)){
				XmlSerializer serializer = new XmlSerializer(typeof(SettingsSaveClass));
				FileStream file = File.Open(settingsPath,FileMode.Open);
				SettingsSaveClass settingsSave = serializer.Deserialize(file) as SettingsSaveClass;
				file.Close();

				settingsValues.LoadSettings(settingsSave);
				
				Debug.Log("Successfully loaded the settings!");
			}
			else {
				Debug.LogWarning("Could not open the file: " + settingsPath);
				Save();
			}
		}
		else {
			Debug.LogWarning("No settings object");
		}

		//Player data
		if (playerStats != null) {
			if (File.Exists(playerStatsPath)) {
				XmlSerializer serializer = new XmlSerializer(typeof(PlayerStatsSaveClass));
				FileStream file = File.Open(playerStatsPath,FileMode.Open);
				PlayerStatsSaveClass playerSave = serializer.Deserialize(file) as PlayerStatsSaveClass;
				file.Close();

				playerStats.LoadStats(playerSave);
				
				Debug.Log("Successfully loaded the player stats!");
			}
			else {
				Debug.LogWarning("Could not open the file: " + playerStatsPath);
				Save();
			}
		}
		else {
			Debug.LogWarning("No player stats object");
		}

		//Trigger data
		if (triggerController != null) {
			if (File.Exists(triggerPath)){
				XmlSerializer serializer = new XmlSerializer(typeof(TriggerSaveClass));
				FileStream file = File.Open(triggerPath,FileMode.Open);
				TriggerSaveClass triggerSave = serializer.Deserialize(file) as TriggerSaveClass;
				file.Close();

				triggerController.LoadTriggers(triggerSave);
				
				Debug.Log("Successfully loaded the triggers!");
			}
			else {
				Debug.LogWarning("Could not open the file: " + triggerPath);
				Save();
			}
		}
		else {
			Debug.LogWarning("No trigger object");
		}
	}
}
