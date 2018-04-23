using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Character")]
public class CharacterEntry : ScrObjLibraryEntry {

	public Sprite defaultColor;
	public Sprite[] poses;


	public override void ResetValues() {
		base.ResetValues();

		defaultColor = null;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		CharacterEntry ce = (CharacterEntry)other;

		defaultColor = ce.defaultColor;
		poses = ce.poses;
	}
}
