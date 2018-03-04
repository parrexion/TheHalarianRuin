using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for a list of MusicEntrys.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/MusicList Variable")]
public class MusicList : ScriptableObject {

	public MusicEntry[] musicClips;


	public AudioClip RandomClip() {
		return musicClips[Random.Range(0,musicClips.Length)].clip;
	}
}
