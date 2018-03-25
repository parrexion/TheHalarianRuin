using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio controller class which plays the background music and sfx.
/// </summary>
public class AudioController : MonoBehaviour {
	
	[Header("Other")]
	public IntVariable currentArea;

	[Header("SFX")]
	public AudioVariable sfxClip;
	public FloatVariable effectVolume;
	public AudioSource efxSource;

	[Header("Music")]
	public AudioVariable battleMusic;
	public AudioVariable dialogueMusic;
	public AudioVariable inventoryMusic;
	public AudioVariable mainMenuMusic;
	public AudioVariable overworldMusic;
	public FloatVariable musicVolume;
	public AudioSource musicSource;

	[Header("Variation values")]
	[MinMaxRange(0.75f,1.5f)]
	public RangedFloat pitchRange = new RangedFloat(0.95f,1.05f);
	
	private bool playingBkg = false;


	public void OnEnable() {
		UpdateVolume();
	}

	public void UpdateVolume() {
		efxSource.volume = Mathf.Clamp01(effectVolume.value);
		musicSource.volume = Mathf.Clamp01(musicVolume.value);
	}

	/// <summary>
	/// Playes the background music or stops the music if clip is null.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlayBackgroundMusic() {
		Debug.Log("MUSIC!!!");
		AudioClip selectedSong = GetCurrentClip();
		
		if (selectedSong == null) {
			musicSource.Stop();
			playingBkg = false;
		}
		else {
			musicSource.clip = selectedSong;
			musicSource.Play();
			playingBkg = true;
		}
	}

	AudioClip GetCurrentClip(){
		switch((Constants.SCENE_INDEXES)currentArea.value)
		{
			case Constants.SCENE_INDEXES.BATTLE:
				return battleMusic.value;
			case Constants.SCENE_INDEXES.DIALOGUE:
				return dialogueMusic.value;
			case Constants.SCENE_INDEXES.INVENTORY:
				return inventoryMusic.value;
			case Constants.SCENE_INDEXES.MAINMENU:
				return mainMenuMusic.value;
			
			default:
				return overworldMusic.value;
		}
	}

	/// <summary>
	/// Playes the background music or stops the music if clip is null.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PauseBackgroundMusic() {
		if (musicSource.clip == null)
			return;
		if( playingBkg) {
			musicSource.Pause();
			playingBkg = false;
		}
		else {
			musicSource.Play();
			playingBkg = true;
		}
	}

	/// <summary>
	/// Plays a single audio clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	private void PlaySingle(AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play();
	}

	/// <summary>
	/// Plays the next sfx clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySfx() {
		if (sfxClip.value != null) {
			efxSource.clip = sfxClip.value;
			efxSource.Play();
		}
	}

	/// <summary>
	/// Plays a random sfx from the list with a random pitch
	/// </summary>
	/// <param name="clips">Clips.</param>
	public void RandomizeSfx(AudioClip[] clips) {
		int randomIndex = Random.Range(0,clips.Length);
		float randomPitch = Random.Range(pitchRange.minValue,pitchRange.maxValue);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];

		efxSource.Play();
	}
}
