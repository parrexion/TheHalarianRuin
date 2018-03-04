using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChapter : MonoBehaviour {

	public Constants.OverworldArea activeArea;
	public Constants.RoomNumber roomNumber = 0;

	public List<Transform> containers = new List<Transform>();
	public List<string> chapterIDs = new List<string>();


	// Use this for initialization
	void Awake () {
		SetupTriggers(false);
	}

	public bool SetupTriggers(bool editorCall) {
		if (!editorCall && transform.childCount == containers.Count){
			Debug.Log("Nothing to update for " + activeArea.ToString());
			return false;
		}

		containers = new List<Transform>();
		chapterIDs = new List<string>();

		Transform child;
		for (int i = 0; i < transform.childCount; i++) {
			child = transform.GetChild(i);
			chapterIDs.Add(child.name);
			containers.Add(child);
		}
		if (chapterIDs.Count > 0 && chapterIDs[0] != "ChangeMap")
			Debug.LogError("This section does not contain a ChangeMap object at the top!");

		return true;
	}

	/// <summary>
	/// Activates the chapter with the given id and deactivates the rest.
	/// </summary>
	/// <param name="chapterID"></param>
	public void ActivateSection(string chapterID, Constants.OverworldArea sceneIndex, Constants.RoomNumber number) {

		if (containers.Count == 0){
			Debug.LogWarning("Empty area");
			return;
		}
		bool state = (sceneIndex == activeArea && roomNumber == number);
		containers[0].gameObject.SetActive(state);
		for (int i = 1; i < containers.Count; i++) {
			Debug.Log("Checking " + chapterIDs[i] + ", object pos " + i + ", Index: " + activeArea);
			containers[i].gameObject.SetActive(state && chapterID == chapterIDs[i]);
		}
	}
}
