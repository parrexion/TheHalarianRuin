using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/OneLiner")]
public class OneLiner : ScriptableObject {

	public CharacterEntry character;
	public int pose;
	public string text;

}
