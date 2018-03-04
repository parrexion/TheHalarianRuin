// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TriggerController))]
public class TriggerControllerEditor : Editor {

    string chapter;

    public override void OnInspectorGUI () {
        TriggerController controller = target as TriggerController;
        DrawDefaultInspector();

        if (GUILayout.Button("Update Triggers")){

            UpdateTriggers(controller);
        }

        GUILayout.Space(20);

        GUILayout.Label("Current chapter: " + controller.currentChapter.value);
        chapter = EditorGUILayout.TextField("Current Chapter", chapter);
        Constants.SCENE_INDEXES cArea = (Constants.SCENE_INDEXES)controller.currentScene.value;
        cArea = (Constants.SCENE_INDEXES)EditorGUILayout.EnumPopup("Current Area", (Constants.SCENE_INDEXES)cArea);
        controller.currentScene.value = (int)cArea;
        
        Constants.RoomNumber cRoom = (Constants.RoomNumber)controller.currentRoomNumber.value;
        cRoom = (Constants.RoomNumber)EditorGUILayout.EnumPopup("Room Number", (Constants.RoomNumber)cRoom);
        controller.currentRoomNumber.value = (int)cRoom;

        GUILayout.Space(20);

        if (GUILayout.Button("Reactivate", GUILayout.Height(50))){

            controller.currentChapter.value = chapter;
            ReactivateTriggers(controller);
        }
    }

    void UpdateTriggers(TriggerController con) {
        Debug.Log("Updating triggers");
        Transform triggerParent = con.transform.GetChild(0);
        int size = triggerParent.childCount;
        TriggerChapter[] childrens = triggerParent.GetComponentsInChildren<TriggerChapter>();

        con.sectionList = childrens;
        bool res;
        for (int i = 0; i < size; i++) {
            res = childrens[i].SetupTriggers(true);
            if (res)
                EditorUtility.SetDirty(childrens[i]);
        }

        EditorUtility.SetDirty(con);
        EditorSceneManager.MarkSceneDirty(con.gameObject.scene);
    }

    void ReactivateTriggers(TriggerController con) {
        Debug.Log("Reactivating triggers");

        con.ReactivateTriggers();
        EditorUtility.SetDirty(con);
        EditorSceneManager.MarkSceneDirty(con.gameObject.scene);
    }
}