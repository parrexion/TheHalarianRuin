using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Music")]
public class MusicEntry : ScrObjLibraryEntry {

	public AudioClip clip;


	public override void ResetValues() {
		base.ResetValues();

		clip = null;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		MusicEntry ce = (MusicEntry)other;

		clip = ce.clip;
	}
}
