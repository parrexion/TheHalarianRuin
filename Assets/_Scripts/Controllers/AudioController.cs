using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio controller class which plays the background music and sfx.
/// </summary>
public class AudioController : MonoBehaviour {
	
#region Singleton
	private static AudioController instance;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			instance = this;
			Startup();
		}
	}
#endregion
	
	[Header("Other")]
	public IntVariable currentArea;
	public IntVariable lastPlayedArea;

	[Header("SFX")]
	public AudioQueueVariable sfxQueue;
	public FloatVariable effectVolume;
	public AudioSource[] efxSource;

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
	private int currentSfxTrack;


	void Startup() {
		lastPlayedArea.value = -1;
		UpdateVolume();
	}

	/// <summary>
	/// Updates the volumes to a value between 0 and 1.
	/// </summary>
	public void UpdateVolume() {
		for (int i = 0; i < efxSource.Length; i++) {
			efxSource[i].volume = Mathf.Clamp01(effectVolume.value);
		}
		musicSource.volume = Mathf.Clamp01(musicVolume.value);
	}

	/// <summary>
	/// Plays the music if the area has changed.
	/// </summary>
	public void PlayMusic() {
		PlayBackgroundMusic(false);
	}

	/// <summary>
	/// Forces the background music to update even in the same area.
	/// </summary>
	public void PlayMusicForced() {
		PlayBackgroundMusic(true);
	}

	/// <summary>
	/// Playes the background music or stops the music if clip is null.
	/// </summary>
	/// <param name="clip">Clip.</param>
	void PlayBackgroundMusic(bool forced) {
		AudioClip selectedSong = GetCurrentClip();
		int areaType = GetAreaType((Constants.SCENE_INDEXES)currentArea.value);
		
		if (selectedSong == null) {
			musicSource.Stop();
			playingBkg = false;
		}
		else if (lastPlayedArea.value != areaType || forced) {
			musicSource.clip = selectedSong;
			musicSource.Play();
			playingBkg = true;
		}
		else {
			if (!playingBkg) {
				PauseBackgroundMusic();
			}
		}
		lastPlayedArea.value = areaType;
	}

	/// <summary>
	/// Retrieves the music clip for the current area.
	/// </summary>
	/// <returns></returns>
	AudioClip GetCurrentClip(){
		switch((Constants.SCENE_INDEXES)currentArea.value)
		{
			case Constants.SCENE_INDEXES.BATTLE:
			case Constants.SCENE_INDEXES.SCORE:
				return battleMusic.value;
			case Constants.SCENE_INDEXES.DIALOGUE:
				return dialogueMusic.value;
			case Constants.SCENE_INDEXES.INVENTORY:
			case Constants.SCENE_INDEXES.SHOP:
				return inventoryMusic.value;
			case Constants.SCENE_INDEXES.MAINMENU:
			case Constants.SCENE_INDEXES.STARTUP:
				return mainMenuMusic.value;
			
			default:
				return overworldMusic.value;
		}
	}

	/// <summary>
	/// Returns the type of the area.
	/// </summary>
	/// <param name="area"></param>
	/// <returns></returns>
	int GetAreaType(Constants.SCENE_INDEXES area){
		switch(area)
		{
			case Constants.SCENE_INDEXES.BATTLE:
			case Constants.SCENE_INDEXES.SCORE:
				return 1;
			case Constants.SCENE_INDEXES.DIALOGUE:
				return 2;
			case Constants.SCENE_INDEXES.INVENTORY:
			case Constants.SCENE_INDEXES.SHOP:
				return 3;
			case Constants.SCENE_INDEXES.MAINMENU:
				return 4;
			
			default:
				return 0;
		}
	}

	/// <summary>
	/// Playes the background music or stops the music if clip is null.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PauseBackgroundMusic() {
		if (musicSource.clip == null)
			return;
		if(playingBkg) {
			musicSource.Pause();
			playingBkg = false;
		}
		else {
			musicSource.Play();
			playingBkg = true;
		}
	}

	/// <summary>
	/// Plays the next sfx clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySfx() {
		while(sfxQueue.value.Count > 0) {
			AudioClip sfxClip = sfxQueue.value.Dequeue();
			if (sfxClip != null) {
				Debug.Log("Play track " + currentSfxTrack);
				RandomizePitch();
				efxSource[currentSfxTrack].clip = sfxClip;
				efxSource[currentSfxTrack].Play();
				currentSfxTrack = (currentSfxTrack + 1) % efxSource.Length;
			}
		}
	}

	/// <summary>
	/// Plays the next sfx clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySfxEntry(SfxEntry entry) {
		if (entry != null && entry.clip != null) {
			Debug.Log("Play track " + currentSfxTrack);
			RandomizePitch();
			efxSource[currentSfxTrack].clip = entry.clip;
			efxSource[currentSfxTrack].Play();
			currentSfxTrack = (currentSfxTrack + 1) % efxSource.Length;
		}
	}

	/// <summary>
	/// Plays a random sfx from the list with a random pitch
	/// </summary>
	/// <param name="clips">Clips.</param>
	public void RandomizePitch() {
		float randomPitch = Random.Range(pitchRange.minValue,pitchRange.maxValue);
		efxSource[currentSfxTrack].pitch = randomPitch;
	}
}
