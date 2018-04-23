using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Sfx")]
public class SfxEntry : ScrObjLibraryEntry {

	public AudioClip clip;


	public override void ResetValues() {
		base.ResetValues();

		clip = null;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		SfxEntry ce = (SfxEntry)other;

		clip = ce.clip;
	}
}
