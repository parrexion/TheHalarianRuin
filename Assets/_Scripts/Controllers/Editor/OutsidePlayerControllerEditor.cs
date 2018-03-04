using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(OutsidePlayerController))]
public class OutsidePlayerControllerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if (GUILayout.Button("Set Current Position")){
			OutsidePlayerController pc = target as OutsidePlayerController;
			SetPosition(pc);
		}
	}

    private void SetPosition(OutsidePlayerController pc) {
		Vector3 pos = pc.transform.position;
		pc.posx.value = pos.x;
		pc.posy.value = pos.y;
		Debug.Log("Position set!");
    }
}
