using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.Events;

public class SaveController : MonoBehaviour {

	public bool loadGame = true;

	[Header("Save Files")]
	public IntVariable currentSaveFile;
	public SaveFiles saveFiles = null;

	[Header("Events")]
	public UnityEvent saveCompleteEvent;
	public UnityEvent loadCompleteEvent;
    public UnityEvent startSavingEvent;
    public UnityEvent startLoadingEvent;

	[Header("DEBUG")]
	public BoolVariable forcePlayerPosition;

	private string savePath = "";
	private int fileChecks;


#region Singleton
	public static SaveController instance;

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

#region XML File Handling

	void Initialize() {
		savePath = Application.persistentDataPath+"/saveData.xml";
		Load();
	}

	public void LoadDefaultSaves() {
		savePath = Application.persistentDataPath+"/saveDataReset.xml";
		Load();
	}

	/// <summary>
	/// Updates the save class and saves it to file.
	/// </summary>
	void Save() {
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
		XmlSerializer serializer = new XmlSerializer(typeof(SaveFiles));
		using (XmlWriter xmlWriter = XmlWriter.Create(savePath, xmlWriterSettings)) {
			serializer.Serialize(xmlWriter, saveFiles);
		}
		Debug.Log("Successfully saved the save data!");
		saveCompleteEvent.Invoke();
	}

	/// <summary>
	/// Reads the save file if it exists and sets the values.
	/// Creates a new empty save file if there is none.
	/// </summary>
	void Load() {
		if (!loadGame)
			return;
			
		//Load save data
		if (File.Exists(savePath)){
			XmlSerializer serializer = new XmlSerializer(typeof(SaveFiles));
			FileStream file = File.Open(savePath,FileMode.Open);
			SaveFiles loadedData = serializer.Deserialize(file) as SaveFiles;
			file.Close();

			if (loadedData == null) {
				Debug.LogWarning("Could not open the file: " + savePath);
				saveFiles = new SaveFiles(3);
				Debug.Log("SAVE WAS NULL");
				Save();
			}
			else {
				saveFiles = loadedData;
				Debug.Log("Successfully loaded the save data!");
			}
		}
		else {
			Debug.LogWarning("Could not open the file: " + savePath);
			saveFiles = new SaveFiles(3);
			Debug.Log("SAVE WS NULL");
			Save();
		}
	}

#endregion

	public void NewGameState() {
		currentSaveFile.value = -1;
	}

	/// <summary>
	/// Loads the given save file into the current game values.
	/// </summary>
	/// <param name="fileIndex"></param>
	public void SaveSaveFile() {
		fileChecks = 0;
		if (saveFiles == null) {
			saveFiles = new SaveFiles(3);
		}
		startSavingEvent.Invoke();
		StartCoroutine(WaitForSaveChecks());
	}

	/// <summary>
	/// Loads the given save file into the current game values.
	/// </summary>
	/// <param name="fileIndex"></param>
	public void LoadSaveFile() {
		forcePlayerPosition.value = false;
		fileChecks = 0;
		startLoadingEvent.Invoke();

		StartCoroutine(WaitForLoadChecks());
	}

	/// <summary>
	/// Waits for all the data to load before giving the okay.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForSaveChecks() {
		while(fileChecks < 3) {
			yield return new WaitForSecondsRealtime(0.1f);
		}

		Debug.Log("Start save file " + currentSaveFile.value);
		Save();
		yield break;
	}

	/// <summary>
	/// Waits for all the data to load before giving the okay.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForLoadChecks() {
		while(fileChecks < 3) 
			yield return new WaitForSecondsRealtime(0.1f);

		Debug.Log("Start loaded file " + currentSaveFile.value);
		loadCompleteEvent.Invoke();
		yield break;
	}

	/// <summary>
	/// Called when a load check is received.
	/// </summary>
	public void FileCheckReceived() {
		fileChecks++;
	}

}


/// <summary>
/// Save class which contains all save files for the game.
/// </summary>
[System.Serializable]
public class SaveFiles {

	public int saveCount;
	public SettingsSaveClass settingsSave;
	public PlayerStatsSaveClass[] playerSave;
	public TriggerSaveClass[] triggerSave;

	public SaveFiles() {
		CreateInfo(3);
	}

	public SaveFiles(int saveCount) {
		CreateInfo(saveCount);
	}

	void CreateInfo(int saveCount) {
		this.saveCount = saveCount;
		settingsSave = new SettingsSaveClass();
		playerSave = new PlayerStatsSaveClass[saveCount];
		triggerSave = new TriggerSaveClass[saveCount];
		for (int i = 0; i < saveCount; i++) {
			playerSave[i] = new PlayerStatsSaveClass();
			triggerSave[i] = new TriggerSaveClass();
		}
	}
}
