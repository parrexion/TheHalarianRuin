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

	[Header("Dialogue")]
	public StringVariable currentDialogueText;
	public GameObject dialogueObject;

	[Header("Sound")]
	public SfxEntry battleStartedSfx;
	public SfxEntry changeAreaFadeSfx;
	public SfxEntry dialogueStartedSfx;
	public SfxEntry enterDoorSfx;
	public AudioVariable musicClip;
	public AudioVariable sfxClip;

	[Header("Events")]
	public UnityEvent playMusicEvent;
	public UnityEvent playSfxEvent;
	public UnityEvent dialogueTextChanged;
	

	/// <summary>
	/// Function which starts the start battle animation.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public void StartBattle() {
		screenText.text = "FIGHT!";
		paused.value = true;

		musicClip.value = null;
		playMusicEvent.Invoke();
		sfxClip.value = battleStartedSfx.clip;
		playSfxEvent.Invoke();
	}

	/// <summary>
	/// Plays the dialogue transition sound when a dialogue is triggered.
	/// </summary>
	public void StartDialogue() {
		sfxClip.value = dialogueStartedSfx.clip;
		playSfxEvent.Invoke();
	}

	/// <summary>
	/// Plays the enter door sound sound when the player enters a door or a shop.
	/// </summary>
	public void EnterDoor() {
		sfxClip.value = enterDoorSfx.clip;
		playSfxEvent.Invoke();
	}

	/// <summary>
	/// Plays the dialogue transition sound when a dialogue is triggered.
	/// </summary>
	public void ChangeWithFadeOut() {
		sfxClip.value = changeAreaFadeSfx.clip;
		playSfxEvent.Invoke();
	}

	/// <summary>
	/// Shows the simple dialogue line.
	/// </summary>
	public void ShowSingleDialogue() {
		if (paused.value)
			return;
		paused.value = true;
		dialogueObject.SetActive(true);
		dialogueTextChanged.Invoke();
	}
}
