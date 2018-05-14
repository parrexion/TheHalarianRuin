using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads everything when the game is started and makes sure that all different modules 
/// are loaded in the correct order.
/// </summary>
public class GameStartScript : MonoBehaviour {

	public AreaIntVariable currentArea;
	public UnityEvent mapChangeEvent;


	// Use this for initialization
	void Start () {
		Debug.Log("Area:   " + currentArea.value);
		StartCoroutine(LoadEverything());
	}

	/// <summary>
	/// Loads the different modules in order.
	/// </summary>
	/// <returns></returns>
	IEnumerator LoadEverything() {
		while (MainControllerScript.instance == null)
			yield return null;

		Debug.Log("Maincontroller now exists");

		while (!MainControllerScript.instance.initiated)
			yield return null;

		Debug.Log("Maincontroller now initiated");

		currentArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
		mapChangeEvent.Invoke();

		yield return null;
	}
}
