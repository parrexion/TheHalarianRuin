using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TriggerController : MonoBehaviour {

#region Singleton
    
    public static TriggerController instance = null;
    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

#endregion

    public IntVariable currentSaveFileIndex;
    public StringListVariable newgameTriggers;

    public BoolVariable paused;
    public IntVariable currentScene;
    public IntVariable currentRoomNumber;
    public StringVariable currentChapter;

    public UnityEvent saveCheckEvent;
    public UnityEvent loadCheckEvent;

    private Dictionary<string,bool> triggerStates = new Dictionary<string, bool>();

    [Header("Sections")]
    public TriggerChapter[] sectionList;


    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        ReactivateTriggers();
    }

    public void SetupTriggers() {
        for (int i = 0; i < sectionList.Length; i++) {
            sectionList[i].SetupTriggers(false);
        }
    }

    /// <summary>
    /// Goes through all sections and activates the current section.
    /// </summary>
    public void ReactivateTriggers() {
        Constants.OverworldArea index = (Constants.OverworldArea)currentScene.value;
        Constants.RoomNumber roomNumber = (Constants.RoomNumber)currentRoomNumber.value;
        for (int i = 0; i < sectionList.Length; i++) {
            sectionList[i].ActivateSection(currentChapter.value, index, roomNumber);
        }
    }

    /// <summary>
    /// Checks if the trigger is active or not.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public bool CheckActive(string uuid, bool alwaysActive) {
        if (triggerStates.ContainsKey(uuid))
            return triggerStates[uuid];

        Debug.Log("Adding trigger: " + uuid + ", state: " + alwaysActive);
        triggerStates.Add(uuid, alwaysActive);
        return alwaysActive;
    }

    /// <summary>
    /// Sets the state och the trigger with the given uuid.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="state"></param>
    public void SetActive(string uuid, bool state) {
        if (triggerStates.ContainsKey(uuid))
            triggerStates[uuid] = state;
        else
            triggerStates.Add(uuid, state);
        
        Debug.Log("Trigger: " + uuid + "  :  " + state);
    }


    // SAVING AND LOADING

    public void NewgameTriggers() {
        triggerStates = new Dictionary<string, bool>();
        SetupTriggers();

        for (int i = 0; i < newgameTriggers.values.Length; i++) {
            triggerStates[newgameTriggers.values[i]] = true;
            Debug.Log("Active trigger: " + newgameTriggers.values[i]);
        }

		Debug.Log("NEWGAME");
    }

    /// <summary>
    /// Stores the data in a save class for saving into xml.
    /// </summary>
    /// <returns></returns>
    public void SaveTriggers() {
		TriggerSaveClass triggerSave = new TriggerSaveClass();

        int size = triggerStates.Count;
        List<string> activeTriggers = new List<string>();
        int index = 0;
        foreach(KeyValuePair<string,bool> trigger in triggerStates) {
            if (trigger.Value) {
                activeTriggers.Add(trigger.Key);
                Debug.Log("ACtive: " + trigger.Key);
            }
            index++;
        }
        triggerSave.uuids = activeTriggers.ToArray();

        SaveController.instance.saveFiles.triggerSave[currentSaveFileIndex.value] = triggerSave;
        saveCheckEvent.Invoke();
		Debug.Log("SAVED");
    }

    /// <summary>
    /// Loads the trigger data from the save class.
    /// </summary>
    /// <param name="saveData"></param>
    public void LoadTriggers() {
        TriggerSaveClass triggerSave = SaveController.instance.saveFiles.triggerSave[currentSaveFileIndex.value];
		
        triggerStates = new Dictionary<string, bool>();
        SetupTriggers();

        for (int i = 0; i < triggerSave.uuids.Length; i++) {
            triggerStates[triggerSave.uuids[i]] = true;
            Debug.Log("Active trigger: " + triggerSave.uuids[i]);
        }

        loadCheckEvent.Invoke();
		Debug.Log("LOADED");
    }
}


/// <summary>
/// Save class for the trigger information list.
/// </summary>
[System.Serializable]
public class TriggerSaveClass {
    public string[] uuids;

    public TriggerSaveClass() {
        uuids = new string[0];
    }

    public TriggerSaveClass(StringListVariable newgameTriggers) {
        if (newgameTriggers == null)
            uuids = new string[0];
        else
            uuids = newgameTriggers.values;
    }
}