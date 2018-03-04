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

    public BoolVariable paused;
    public StringListVariable newgameUuid;
    public IntVariable currentScene;
    public IntVariable currentRoomNumber;
    public StringVariable currentChapter;

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
        // SetupTriggers();
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

        bool active = alwaysActive;
        for (int i = 0; i < newgameUuid.values.Length; i++) {
            if (uuid == newgameUuid.values[i]){
                active = true;
                break;
            }
        }
        Debug.Log("Adding trigger: " + uuid + ", state: " + active);
        triggerStates.Add(uuid, active);
        return active;
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
    }


    // SAVING AND LOADING

    /// <summary>
    /// Stores the data in a save class for saving into xml.
    /// </summary>
    /// <returns></returns>
    public TriggerSaveClass SaveTriggers() {
        TriggerSaveClass saveData = new TriggerSaveClass();

        int size = triggerStates.Count;
        saveData.uuids = new string[size];
        saveData.states = new bool[size];
        int index = 0;
        foreach(KeyValuePair<string,bool> trigger in triggerStates) {
            saveData.uuids[index] = trigger.Key;
            saveData.states[index] = trigger.Value;
            index++;
        }

        return saveData;
    }

    /// <summary>
    /// Loads the trigger data from the save class.
    /// </summary>
    /// <param name="saveData"></param>
    public void LoadTriggers(TriggerSaveClass saveData) {

        triggerStates = new Dictionary<string, bool>();

        for (int i = 0; i < saveData.states.Length; i++) {
            triggerStates.Add(saveData.uuids[i],saveData.states[i]);
        }
    }
}


/// <summary>
/// Save class for the trigger information list.
/// </summary>
public class TriggerSaveClass {
    public string[] uuids;
    public bool[] states;
}