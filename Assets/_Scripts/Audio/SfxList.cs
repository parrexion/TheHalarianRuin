using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for a list of SfxEntrys.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/SfxList Variable")]
public class SfxList : ScrObjLibraryEntry {

	public List<SfxEntry> sfxClips = new List<SfxEntry>();


	public AudioClip RandomClip() {
		return sfxClips[Random.Range(0,sfxClips.Count)].clip;
	}

	public override void ResetValues() {
		base.ResetValues();
		sfxClips = new List<SfxEntry>();
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		SfxList ml = (SfxList)other;

		sfxClips = new List<SfxEntry>();
		for (int i = 0; i < ml.sfxClips.Count; i++) {
			sfxClips.Add(ml.sfxClips[i]);
		}
	}
}
