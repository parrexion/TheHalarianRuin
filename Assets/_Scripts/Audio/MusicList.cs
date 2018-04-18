using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for a list of MusicEntrys.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/MusicList Variable")]
public class MusicList : ScrObjLibraryEntry {

	public List<MusicEntry> musicClips = new List<MusicEntry>();


	public AudioClip RandomClip() {
		return musicClips[Random.Range(0,musicClips.Count)].clip;
	}

	public override void ResetValues() {
		base.ResetValues();
		musicClips = new List<MusicEntry>();
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		MusicList ml = (MusicList)other;

		musicClips = new List<MusicEntry>();
		for (int i = 0; i < ml.musicClips.Count; i++) {
			musicClips.Add(ml.musicClips[i]);
		}
	}
}
