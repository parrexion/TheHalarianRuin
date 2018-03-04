// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AreaIntVariable))]
public class AreaIntVariableEditor : Editor {

    Constants.SCENE_INDEXES asdf;

    public override void OnInspectorGUI () {
        AreaIntVariable areaInt = target as AreaIntVariable;
        GUILayout.Label("Value : " + areaInt.value.ToString());
        asdf = (Constants.SCENE_INDEXES)EditorGUILayout.EnumPopup("Area", (Constants.SCENE_INDEXES)areaInt.value);
        areaInt.value = (int)asdf;
    }
}