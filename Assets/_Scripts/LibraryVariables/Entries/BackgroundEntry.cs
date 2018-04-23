using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Background")]
public class BackgroundEntry : ScrObjLibraryEntry {

	public Sprite sprite;


	public override void ResetValues() {
		base.ResetValues();

		sprite = null;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		BackgroundEntry ce = (BackgroundEntry)other;

		sprite = ce.sprite;
	}
}
