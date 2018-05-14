using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// The main controller class which contains a reference to most of the global modules 
/// used in the game. 
/// </summary>
public class MainControllerScript : MonoBehaviour {

#region Singleton
	public static MainControllerScript instance;

	/// <summary>
	/// Loads all the modules.
	/// </summary>
	void Awake () {
		if (instance != null) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(transform.gameObject);
		instance = this;
		battleGUI = GetComponent<BattleGUIController>();
		StartCoroutine("WaitForInitiate");
	}
#endregion

	public bool initiated;

	public AreaInfoValues areaInfo;
	public IntVariable currentScene;
	public IntVariable currentRoom;

	[Header("Fade Out")]
	public bool useFadeOut = true;
	public bool fading = false;
	public FloatVariable fadeOutTime;
	public UnityEvent fadeOutEvent;

	[HideInInspector] public BattleGUIController battleGUI;
	

	/// <summary>
	/// Waits to make sure that the necessary modules are loaded before the last modules are initiated.
	/// </summary>
	/// <returns></returns>
	void WaitForInitiate() {
		initiated = true;
		Debug.Log("Maincontroller is ready");
	}

	/// <summary>
	/// Moves to the next scene set by the other script using fade out if 
	/// useFadeOut is true.
	/// </summary>
	public void MoveToScene() {
		if (fading)
			return;
		fading = true;

		StartCoroutine(WaitForFadeOut(useFadeOut));
	}

	/// <summary>
	/// Moves to the dialogue scene using fade out.
	/// </summary>
	public void StartDialogue() {
		currentScene.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
		StartCoroutine(WaitForFadeOut(true));
	}

	/// <summary>
	/// Moves to the battle scene without fadeout.
	/// </summary>
	public void StartBattle() {
		currentScene.value = (int)Constants.SCENE_INDEXES.BATTLE;
		StartCoroutine(WaitForFadeOut(false));
	}

	/// <summary>
	/// Waits for the screen to fade out before returning to the game.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForFadeOut(bool isFade) {
		if (isFade) {
			fadeOutEvent.Invoke();
			yield return new WaitForSeconds(fadeOutTime.value);
		}

		fading = false;

		AreaValue value = areaInfo.GetArea(currentScene.value, currentRoom.value);
		Debug.Log("Changed to area:  " + value.locationName);
		SceneManager.LoadScene(value.sceneID);

		yield break;
	}

	/// <summary>
	/// Wait for a while then move on to the battle screen.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	private IEnumerator BattleDelay(float time){
		yield return new WaitForSeconds(time);

		currentScene.value = (int)Constants.SCENE_INDEXES.BATTLE;
		AreaValue value = areaInfo.GetArea(currentScene.value, currentRoom.value);
		Debug.Log("Changed to area:  " + value.locationName);
		SceneManager.LoadScene(value.sceneID);
	}
}




public static class AppHelper {

	public static void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}