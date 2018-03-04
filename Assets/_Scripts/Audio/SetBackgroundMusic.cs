using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which sets the background music to one of the songs in the list.
/// </summary>
public class SetBackgroundMusic : MonoBehaviour {

	public bool randomSong = true;
	public MusicList musicList;
	public AudioVariable backgroundMusic;
	public UnityEvent backgroundMusicChanged;

	void Start() {
		if (musicList == null)
			Debug.Log("No audio clip defined! Intended?");

		backgroundMusic.value = musicList.RandomClip();
		backgroundMusicChanged.Invoke();
	}

}
