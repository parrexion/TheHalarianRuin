using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for a list of SfxEntrys.
/// </summary>
[CreateAssetMenu(menuName="List ScrObj Variables/SfxList Variable")]
public class SfxList : ScriptableObject {

	public SfxEntry[] sfxClips;


	public AudioClip RandomClip() {
		return sfxClips[Random.Range(0,sfxClips.Length)].clip;
	}
}
