// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OWTrigger))]
public class OWTriggerEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        GUILayout.Space(20);
        if (GUILayout.Button("Trigger", GUILayout.Height(50))){

            OWTrigger controller = target as OWTrigger;
            controller.Trigger();

            Debug.Log("Triggered the trigger!");
        }
    }
}

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        GUILayout.Space(20);
        if (GUILayout.Button("Trigger", GUILayout.Height(50))){

            OWTrigger controller = target as OWTrigger;
            controller.Trigger();

            Debug.Log("Triggered the trigger!");
        }
    }
}