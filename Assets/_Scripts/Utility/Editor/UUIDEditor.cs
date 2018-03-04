// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UUID))] public class UUIDEditor : Editor {

    public override void OnInspectorGUI () {
        UUID uuid = target as UUID;
        EditorGUILayout.SelectableLabel(uuid.uuid);
        if (GUILayout.Button("New UUID")) {
            uuid.uuid = System.Guid.NewGuid().ToString();
        }
    }
}