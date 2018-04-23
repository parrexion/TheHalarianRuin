using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Music")]
public class MusicEntry : ScrObjLibraryEntry {

	public AudioClip clip;
	public int identifier;


	public override void ResetValues() {
		base.ResetValues();

		clip = null;
		identifier = -1;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		MusicEntry me = (MusicEntry)other;

		clip = me.clip;
		identifier = me.identifier;
	}
}
