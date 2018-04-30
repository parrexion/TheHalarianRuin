using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Overworld trigger which starts a battle when colliding with battle triggers.
/// </summary>
public class InitiateBattleScript : MonoBehaviour {

	public Text screenText;
	public BoolVariable paused;
	public AreaIntVariable currentArea;

	[Header("Sound")]
	public SfxEntry battleStartedSfx;
	public AudioVariable musicClip;
	public AudioVariable sfxClip;

	[Header("Events")]
	public UnityEvent playMusicEvent;
	public UnityEvent playSfxEvent;
	public UnityEvent changeMapEvent;
	

	/// <summary>
	/// Function which starts the start battle animation.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public void StartBattle(float time) {
		StartCoroutine(BattleDelay(time));
	}

	/// <summary>
	/// Show the fight text and move on to the battle screen after a while.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	private IEnumerator BattleDelay(float time){
		screenText.text = "FIGHT!";
		paused.value = true;

		musicClip.value = null;
		playMusicEvent.Invoke();
		sfxClip.value = (battleStartedSfx != null) ? battleStartedSfx.clip : null;
		playSfxEvent.Invoke();

		yield return new WaitForSeconds(time);
		currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
		changeMapEvent.Invoke();
	}
}
